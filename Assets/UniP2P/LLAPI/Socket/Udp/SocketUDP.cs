using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UniRx;
using UniRx.Async;
using LumiSoft.Net.STUN.Client;
using UniP2P.Debug;

namespace UniP2P.LLAPI
{
    public class SocketUdp : ISocket
    {
        protected UdpClient UdpSocket;

        protected static List<UdpConnection> UdpConnections = new List<UdpConnection>();

        public static int GetUdpConnectionsCount()
        {
            return UdpConnections.Count;
        }

        public static UdpConnection[] GetUdpConnections()
        {
            return UdpConnections.ToArray();
        }

        public int BindPort()
        {
            return bindPort;
        }

        private int bindPort
        {
            get; set;
        }

        protected IPEndPoint STUNIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        protected bool isSendLoop;
        protected bool isReceiveLoop;

        public void Init()
        {
            SocketInit();
        }

        public void SocketInit()
        {
            UdpSocket = new UdpClient(new IPEndPoint(IPAddress.Any, UniP2PManager.AdvancedSettings.Port));
            string s = UdpSocket.Client.LocalEndPoint.ToString();
            bindPort = int.Parse(s.Split(':')[1]);
            isSendLoop = true;
            isReceiveLoop = true;
            STUN();
            StartReceiveAsync();
            StartSendAsync();
        }

        public async void StartSendAsync()
        {
            Debugger.Log("[SocketUDP] Start SendLoop");
            await SendLoopAsync();
        }

        public async UniTask SendLoopAsync()
        {
            int wait = Convert.ToInt32(Math.Floor((1f / UniP2PManager.AdvancedSettings.TickRate) * 1000));
            while (isSendLoop)
            {
                foreach (var u in UdpConnections.ToArray())
                {
                    //await u.SendReliable(UdpSocket);
                    await u.SendUnreliable(UdpSocket);
                }
                try
                {
                    await UniTask.Delay(wait);
#if UNITY_EDITOR
                    if (!UnityEditor.EditorApplication.isPlaying)
                    {
                        Shutdown();
                    }
#endif
                }
                catch (Exception ex)
                {
                    Debugger.Error(ex.Message);
                    return;
                }
            }
        }

        public void Shutdown()
        {
            Debugger.Log("Shutdown");
            isSendLoop = false;
            isReceiveLoop = false;
            UdpSocket.Close();
        }      

        #region STUN

        public void STUN()
        {
            if (UniP2PManager.AdvancedSettings.isSTUN)
            {
                STUN_Result result = STUN_Client.Query(UniP2PManager.AdvancedSettings.STUNURL, UniP2PManager.AdvancedSettings.STUNPort, UdpSocket);
                Debugger.Log("[STUN] NetType:" + result.NetType.ToString());
                if (result.NetType == STUN_NetType.UdpBlocked)
                {
                    Debugger.Error("[STUN] UDP blocked");
                    HLAPI.NetworkErrorCanvas.isError = true;
                }
                else if (result.NetType == STUN_NetType.Symmetric || result.NetType == STUN_NetType.SymmetricUdpFirewall)
                {
                    Debugger.Log("[STUN] Symmetric NAT");
                    HLAPI.NetworkErrorCanvas.isError = true;
                }
                else
                {
                    IPEndPoint publicEP = result.PublicEndPoint;
                    Debugger.Log("[STUN] IPEndPoint:" + publicEP.Address + ":" + publicEP.Port);
                    STUNIPEndPoint = publicEP;
                }
            }
        }
        public IPEndPoint GetSTUNResult()
        {
            return STUNIPEndPoint;
        }

        #endregion

        #region Receiver

        private async void StartReceiveAsync()
        {
            Debugger.Log("[SocketUDP] Start ReceiveLoop");
            await ListenLoopAsync();
        }

        private async UniTask ListenLoopAsync()
        {
            while (isReceiveLoop)
            {
                try
                {
                    var result = await UdpSocket.ReceiveAsync();
                    if (result != null)
                    {
                        await OnReceiveCompletedAsync(result.Buffer, result.RemoteEndPoint);
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    Debugger.Log("[SocketUDP] Close");
                    break;
                }
            }
        }

        private async UniTask OnReceiveCompletedAsync(byte[] buffer, IPEndPoint remote)
        {
            if (Equals(remote, UniP2PManager.PrivateIPEndPoint) || Equals(remote, UniP2PManager.StunIPEndPoint))
            {
                Debugger.Warning("[UniP2PManager] IPEndPoint is MySelf.");
                return;
            }
            UdpPacket packet = null;
            try
            {
                packet = UdpPacket.Deserialize(buffer);
            }
            catch (Exception ex)
            {
                //Debugger.Warning(ex.Message + remote.ToString());
                return;
            }

            if (packet.PeerID != null)
            {
                var udp = GetUdpConnection(packet.PeerID);

                if (udp != null)
                {
                    if (udp.State == UdpConnectionState.Connected)
                    {
                        if (packet.isEncrypt())
                        {
                            var decrypt = AES.Decrypt(packet.UdpPacketL2, udp.AESKey, packet.UdpPacketL2IV);
                            var l2 = UdpPacketL2.Deserialize(decrypt);

                            if (l2.ACKNumber != 0)
                            {
                                udp.ReceiveACKPacket(l2.ACKNumber);
                            }

                            //Reliable
                            if (l2.PacketNumber != 0)
                            {
                                //Reply ACK

                            }

                            foreach (var command in l2.Commands)
                            {
                                if (command.P2PEventType == CommandType.Disconnect)
                                {
                                    Debugger.Log("[SocketUDP] DisConnect PeerID:" + udp.Peer.ID);
                                    udp.DisConnect();
                                    UdpConnections.Remove(udp);
                                    UniP2PManager.RemovePeer(udp.Peer);
                                    return;
                                }
                                else if (command.P2PEventType == CommandType.HeartBeat)
                                {
                                    udp.ReceiveHeartBeat();
                                }
                                else if (command.P2PEventType == CommandType.Nothing || command.P2PEventType == CommandType.DataEvent)
                                {
                                    var e = new P2PEventArgs(udp.Peer, command.P2PEventType, command.Value);
                                    ReceiveSubject.OnNext(e);
                                }
                            }
                        }
                        else
                        {
                            var l2 = UdpPacketL2.Deserialize(packet.UdpPacketL2);
                            foreach (var command in l2.Commands)
                            {
                                if (command.P2PEventType == CommandType.HeartBeat)
                                {
                                    udp.ReceiveHeartBeat();
                                }
                            }
                        }
                    }
                    else if (udp.State == UdpConnectionState.KeyExchange)
                    {
                        var l2 = UdpPacketL2.Deserialize(packet.UdpPacketL2);
                        foreach (var command in l2.Commands)
                        {
                            if (command.P2PEventType == CommandType.RequestKey)
                            {
                                await udp.CreateKey(command.Value, UdpSocket, this);
                                break;
                            }
                            else if (command.P2PEventType == CommandType.AcceptKey)
                            {
                                udp.AcceptKey(command.Value, this);
                                break;
                            }
                        }
                    }

                    else if (udp.State == UdpConnectionState.RequestSend)
                    {
                        var l2 = UdpPacketL2.Deserialize(packet.UdpPacketL2);
                        foreach (var command in l2.Commands)
                        {
                            if (command.P2PEventType == CommandType.RequestAccept)
                            {
                                await ReceiveConnectAcceptEvent(udp, remote);
                                break;
                            }
                        }
                    }
                    else if (udp.State == UdpConnectionState.DisConnect)
                    {
                        udp.DisConnect();
                        return;
                    }
                }
                else
                {
                    //Not Found Udp Connection
                    var l2 = UdpPacketL2.Deserialize(packet.UdpPacketL2);

                    foreach (var command in l2.Commands)
                    {
                        if (command.P2PEventType == CommandType.ConnectRequest)
                        {
                            var peer = new Peer
                            {
                                ID = packet.PeerID,
                                IPEndPoint = remote,
                            };
                            await ReceiveConnectRequestEvent(peer);
                        }
                        else if (command.P2PEventType == CommandType.PingRequest)
                        {
                            await SendPingResponsePacketAsync(remote, PingPacket.Deserialize(command.Value).PingID);
                        }
                        else if (command.P2PEventType == CommandType.PingResponse)
                        {
                            ReceivePingResponsePacketAsync(PingPacket.Deserialize(command.Value).PingID);
                        }
                    }
                }
            }
            else
            {
                var l2 = UdpPacketL2.Deserialize(packet.UdpPacketL2);

                foreach (var command in l2.Commands)
                {
                    if (command.P2PEventType == CommandType.PingRequest)
                    {
                        await SendPingResponsePacketAsync(remote, PingPacket.Deserialize(command.Value).PingID);
                    }
                    else if (command.P2PEventType == CommandType.PingResponse)
                    {
                        ReceivePingResponsePacketAsync(PingPacket.Deserialize(command.Value).PingID);
                    }
                }
            }   
        }

        private Subject<P2PEventArgs> ReceiveSubject = new Subject<P2PEventArgs>();

        Subject<P2PEventArgs> ISocket.OnReceiveDataChanged()
        {
            return ReceiveSubject;
        }
        #endregion

        #region Peer

        public async UniTask<Peer> ConnectPeerAsync(IPEndPoint ip, string peerid = "", int localport = 0)
        {
            var peer = new Peer
            {
                State = PeerState.Connecting,
                ID = peerid,
            };
            var udp = new UdpConnection(peer);
            UdpConnections.Add(udp);
            udp.State = UdpConnectionState.RequestSend;

            int i = 0;
            do
            {
                await SendConnectRequestEvent(ip);

                if (i >= 3 && localport != 0)
                {
                    Debugger.Log("[SocketUDP] LAN Connect Port:" + localport);
                    await SendConnectRequestEvent(ip, true, localport);
                }
                if (i >= 5)
                {
                    Debugger.Error("[SocketUDP] Timeout: " + ip.ToString());
                    UniP2PManager.RemovePeer(peer);
                    return null;
                }
                await UniTask.Delay(1000);
                i++;
            }
            while (peer.State != PeerState.Connected);

            return peer;
        }

        public async UniTask SendPacketAsync(Peer peer, byte[] data, CommandType p2PEvent = CommandType.Nothing, SocketQosType qosType = SocketQosType.Unreliable)
        {
            await SendPacketDataAsync(peer, data, p2PEvent, qosType);
        }

        public async UniTask DisConnectAsync(Peer peer)
        {
            if (peer.IPEndPoint != null)
            {
                var udp = GetUdpConnection(peer.ID);
                byte[] b = { };
                await SendAsync(udp, b, CommandType.Disconnect, SocketQosType.Unreliable);
                udp.DisConnect();
                UdpConnections.Remove(udp);
                var e = new P2PEventArgs(peer, CommandType.Disconnect, null);
                ReceiveSubject.OnNext(e);
            }
        }

        #endregion

        #region UdpConnection
        public async UniTask SendAsync(UdpConnection udp, byte[] data, CommandType p2PEvent = CommandType.Nothing, SocketQosType qosType = SocketQosType.Unreliable)
        {
            Command packetObject = new Command();
            packetObject.P2PEventType = p2PEvent;
            packetObject.Value = data;
            if (udp != null)
            {
                if (qosType == SocketQosType.Reliable)
                {
                    await udp.AddReliableCommand(packetObject);
                }
                else if (qosType == SocketQosType.Unreliable)
                {
                    udp.AddUnreliableCommand(packetObject);
                }
            }
        }

        private async UniTask SendConnectRequestEvent(IPEndPoint ip, bool isBroadcast = false, int localport = 0)
        {
            Command command = new Command();
            command.P2PEventType = CommandType.ConnectRequest;
            command.Value = null;
            var p = new Command[1];
            p[0] = command;

            var l2 = new UdpPacketL2
            {
                PacketNumber = 0,
                ACKNumber = 0,
                Commands = p,
            }.Serialize();

            var packet = new UdpPacket
            {
                PeerID = UniP2PManager.MyPeerID,
                UdpPacketL2 = l2,
                UdpPacketL2IV = null,
            }.Serialize();

            if (isBroadcast && localport != 0)
            {
                UdpSocket.EnableBroadcast = true;
                await UdpSocket.SendAsync(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, localport));
                PacketCapture.Write("Broadcast:" + localport, packet.Length, "ConnectRequestEvent");
                UdpSocket.EnableBroadcast = false;
                await UdpSocket.SendAsync(packet, packet.Length, new IPEndPoint(UniP2PManager.PrivateIPEndPoint.Address, localport));
                PacketCapture.Write(UniP2PManager.PrivateIPEndPoint.Address.ToString() + ":" + localport, packet.Length, "ConnectRequestEvent");
            }
            else
            {
                await UdpSocket.SendAsync(packet, packet.Length, ip);
            }
        }

        private async UniTask ReceiveConnectRequestEvent(Peer peer)
        {
            var connection = new UdpConnection(peer);
            UdpConnections.Add(connection);
            await SendConnectAcceptEvent(connection);
            connection.State = UdpConnectionState.KeyExchange;
            var rtt = await GetRTTAsync(peer.IPEndPoint);
            connection.RTT = rtt;
            Debugger.Log("[SocketUDP] Receive Connect Request Event:" + peer.IPEndPoint.Address.ToString() + ":" + peer.IPEndPoint.Port.ToString());
        }

        private async UniTask SendConnectAcceptEvent(UdpConnection udp)
        {
            Command command = new Command();
            command.P2PEventType = CommandType.RequestAccept;
            command.Value = null;
            var p = new Command[1];
            p[0] = command;

            var l2 = new UdpPacketL2
            {
                PacketNumber = 0,
                ACKNumber = 0,
                Commands = p,
            }.Serialize();

            var packet = new UdpPacket
            {
                PeerID = UniP2PManager.MyPeerID,
                UdpPacketL2 = l2,
                UdpPacketL2IV = null,
            }.Serialize();

            await UdpSocket.SendAsync(packet, packet.Length, udp.Peer.IPEndPoint);
            PacketCapture.Write(udp.Peer.IPEndPoint.ToString(), packet.Length, "ConnectAcceptEvent");
            Debugger.Log("[SocketUDP] SendConnectAcceptEvent: dist:" + udp.Peer.IPEndPoint.ToString());
        }

        private async UniTask ReceiveConnectAcceptEvent(UdpConnection udp, IPEndPoint ip)
        {
            udp.Peer.IPEndPoint = ip;
            udp.State = UdpConnectionState.KeyExchange;
            Debugger.Log("[SocketUDP] ReceiveConnectAcceptEvent: dist:" + udp.Peer.IPEndPoint.ToString());
            await udp.RequestKey(UdpSocket);
            var rtt = await GetRTTAsync(udp.Peer.IPEndPoint);
            udp.RTT = rtt;
            Debugger.Log("[SocketUDP] Get RTT: " + rtt);
        }

        private async UniTask<int> GetRTTAsync(IPEndPoint remote)
        {
            List<int> secs = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                secs.Add(await SendPingPacketAsync(remote));
                await UniTask.Delay(10);
            }
            secs.Sort();
            if (secs.ToArray().Length % 2 == 1)
            {
                return secs[(secs.ToArray().Length - 1) / 2] + 10;
            }
            else
            {
                return (secs[(secs.ToArray().Length / 2) - 1] + secs[secs.ToArray().Length / 2]) / 2 + 10;
            }
        }

        private UdpConnection GetUdpConnection(string peerid)
        {
            if (peerid == UniP2PManager.MyPeerID)
            {
                return null;
            }
            foreach (var p in UdpConnections)
            {
                if (p.Peer.ID == peerid)
                {
                    return p;
                }
            }
            return null;
        }

        private UdpConnection GetUdpConnection(IPEndPoint ip)
        {
            foreach (var p in UdpConnections)
            {
                if (Equals(p.Peer.IPEndPoint, ip))
                {
                    return p;
                }
            }
            return null;
        }

        private UdpConnection GetUdpConnection(string peerid, IPEndPoint ip)
        {
            foreach (var p in UdpConnections)
            {
                if (Equals(p.Peer.IPEndPoint, ip))
                {
                    if (Equals(p.Peer.ID, peerid))
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        public async UniTask SendPacketDataAsync(Peer peer, byte[] data, CommandType p2PEvent = CommandType.Nothing, SocketQosType qosType = SocketQosType.Unreliable)
        {
            foreach (var connection in UdpConnections)
            {
                if (connection.Peer == peer)
                {
                    await SendAsync(GetUdpConnection(peer.ID), data, p2PEvent, qosType);
                    PacketCapture.Write(peer.IPEndPoint.ToString(), data.Length, "PacketData");
                    return;
                }
            }
        }

        public async UniTask SendHeartBeatAsync(UdpConnection udp)
        {
            byte[] b = { };
            Debugger.Log("[SocketUDP] SendHeartBeatAsync:" + udp.Peer.IPEndPoint.ToString());
            await SendAsync(udp, b, CommandType.HeartBeat, SocketQosType.Unreliable);
            PacketCapture.Write(udp.Peer.IPEndPoint.ToString(), b.Length, "HeartBeat");
        }

        #endregion

        #region IPBase
        public async UniTask SendEmptyPacketAsync(IPEndPoint ip)
        {
            byte[] buf = { };
            try
            {
                await UdpSocket.SendAsync(buf, buf.Length, ip);
                PacketCapture.Write(ip.ToString(), buf.Length, "EmptyPacket");
            }
            catch (ObjectDisposedException ex)
            {
                //Debugger.Log("[SocketUDP] Closed");
            }
        }

        #endregion

        #region Ping
        private int pingid = 0;
        private Dictionary<int, bool> PingTemp = new Dictionary<int, bool>();

        public async UniTask<int> SendPingPacketAsync(IPEndPoint ip)
        {
            int ms = 0;
            try
            {
                var objs = new Command[1];
                objs[0] = new Command { P2PEventType = CommandType.PingRequest, Value = new PingPacket { PingID = pingid }.Serialize() };
                var u = new UdpPacketL2
                {
                    ACKNumber = 0,
                    PacketNumber = 0,
                    Commands = objs
                }.Serialize();

                var packet = new UdpPacket
                {
                    PeerID = null,
                    UdpPacketL2 = u,
                    UdpPacketL2IV = null
                }.Serialize();
                await UdpSocket.SendAsync(packet, packet.Length, ip);
                PacketCapture.Write(ip.ToString(), packet.Length, "Ping");
                var id = pingid;
                PingTemp.Add(id, false);
                pingid++;
                do
                {
                    await UniTask.Delay(1);
                    ms++;
                }
                while (PingTemp[id]);
                PingTemp.Remove(id);
                return ms;
            }
            catch (ObjectDisposedException ex)
            {
                Debugger.Error(ex.Message);
                Debugger.Log("[SocketUDP] Closed");
            }

            return -1;
        }

        private async UniTask SendPingResponsePacketAsync(IPEndPoint ip, int id)
        {
            try
            {
                var commands = new Command[1];
                commands[0] = new Command
                { P2PEventType = CommandType.PingResponse, Value = new PingPacket { PingID = id }.Serialize() };
                var l2 = new UdpPacketL2
                {
                    ACKNumber = 0,
                    PacketNumber = 0,
                    Commands = commands,
                }.Serialize();

                var packet = new UdpPacket
                {
                    PeerID = null,
                    UdpPacketL2 = l2,
                    UdpPacketL2IV = null
                }.Serialize();

                await UdpSocket.SendAsync(packet, packet.Length, ip);
                PacketCapture.Write(ip.ToString(), packet.Length, "PingResponse");
            }
            catch (ObjectDisposedException ex)
            {
                Debugger.Error(ex.Message);
                Debugger.Log("[SocketUDP] Closed");
            }
        }

        private void ReceivePingResponsePacketAsync(int id)
        {
            PingTemp[id] = true;
        }

        #endregion

    }
}