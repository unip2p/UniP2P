using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UniRx;
using UniRx.Async;
using System.Linq;

namespace UniP2P.LLAPI
{
    public class UdpConnection : IDisposable
    {
        public Peer Peer;

        public UdpConnectionState State;

        private List<Command> UnreliableCommandBuffer = new List<Command>();

        private List<Command> ReliableCommandBuffer = new List<Command>();

        /// <summary>
        /// My Sent PacketCount
        /// </summary>
        private ulong MyPacketCount = 1;

        /// <summary>
        /// Peer ACK Packet Count
        /// </summary>
        private ulong PeerWaitPacketNumber= 1;

        public int RTT = 10;

        private RSA RSA;

        public byte[] AESKey;

        private bool isTickReliablePacket;

        public UdpConnection(Peer peer)
        {
            Peer = peer;
            State = UdpConnectionState.RequestSend;
        }

        public void DisConnect()
        {
            isTickReliablePacket = false;
            UnreliableCommandBuffer.Clear();
            ReliableCommandBuffer.Clear();
            SentReliableBuffer.Clear();
            State = UdpConnectionState.DisConnect;
        }

        #region KeyExchange

        public async UniTask RequestKey(UdpClient UdpSocket)
        {
            State = UdpConnectionState.KeyExchange;
            RSA = new RSA();
            var publickey = RSA.RequestKey();
            
            Command obj = new Command
            {
                P2PEventType = CommandType.RequestKey,
                Value = publickey.Serialize(),
            };
            var array = new Command[1];
            array[0] = obj;
            var l2 = new UdpPacketL2
            {
                Commands = array,
                PacketNumber = 0,
                ACKNumber = 0,
            }.Serialize();

            var buf = new UdpPacket
            {
                PeerID = UniP2PManager.MyPeerID,
                UdpPacketL2 = l2,
                UdpPacketL2IV = null
            }.Serialize();

            await UdpSocket.SendAsync(buf, buf.Length, Peer.IPEndPoint);
            PacketCapture.Write(Peer.IPEndPoint.ToString(),buf.Length, "RequestKey");
        }

        public async UniTask CreateKey(byte[] buffer, UdpClient udpSocket ,SocketUdp socketUdp)
        {
            var publicKey = RSAPublicKey.Deserialize(buffer);
            RSA = new RSA();
            var key = RSA.CreateKey(publicKey.Modules, publicKey.Exponent);
            AESKey = key.aeskey;

            Command obj = new Command
            {
                P2PEventType = CommandType.AcceptKey,
                Value = key.encrypted,
            };
            var array = new Command[1];
            array[0] = obj;

            var l2 = new UdpPacketL2
            {
                Commands = array,
                PacketNumber = 0,
                ACKNumber = 0,
            }.Serialize();

            var buf = new UdpPacket
            {
                PeerID = UniP2PManager.MyPeerID,
                UdpPacketL2 = l2,
                UdpPacketL2IV = null
            }.Serialize();
            
            await udpSocket.SendAsync(buf, buf.Length, Peer.IPEndPoint);
            PacketCapture.Write(Peer.IPEndPoint.ToString(), buf.Length, "CreateKey");
            State = UdpConnectionState.Connected;
            Peer.State = PeerState.Connected;
            InitHeartBeatAsync(socketUdp);
        }

        public void AcceptKey(byte[] buffer, SocketUdp socketUdp)
        {
            AESKey = RSA.AcceptKey(buffer);
            State = UdpConnectionState.Connected;
            Peer.State = PeerState.Connected;
            InitHeartBeatAsync(socketUdp);
        }

        public byte[] Decrypt(byte[] value, byte[] iv)
        {
            return AES.Decrypt(value, AESKey, iv);
        }
        
        #endregion

        #region HeartBeat

        public async void InitHeartBeatAsync(SocketUdp socket)
        {
            await UniTask.WhenAll(HeartBeatAsync(socket), CheckHeartBeatAsync());
        }

        private bool isReceiveHeartBeat;

        public async UniTask HeartBeatAsync(SocketUdp socket)
        {
            try
            {
                while (State == UdpConnectionState.Connected)
                {
                    await socket.SendHeartBeatAsync(this);
                    await UniTask.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.Debugger.Warning(ex);
                await HeartBeatAsync(socket);
            }
        }

        private async UniTask CheckHeartBeatAsync()
        {
            while (State == UdpConnectionState.Connected)
            {
                await UniTask.Delay(10000);

                if (isReceiveHeartBeat)
                {
                    isReceiveHeartBeat = false;
                }
                else
                {
                    Debug.Debugger.Log("[UdpConnection] Time Out Peer:" + Peer.ID);
                    await UniP2PManager.DisConnectPeerAsync(Peer);
                }
            }
        }

        public void ReceiveHeartBeat()
        {
            Debug.Debugger.Log("[UdpConnection] ReceiveHeartBeat:" + Peer.ID);
            isReceiveHeartBeat = true;
        }

        #endregion

        #region Unreliable

        public void AddUnreliableCommand(Command obj)
        {
            UnreliableCommandBuffer.Add(obj);
        }

        public async UniTask SendUnreliable(UdpClient UdpSocket)
        {
            if (UnreliableCommandBuffer.ToArray().Length != 0)
            {
                var l2 = new UdpPacketL2
                {
                    PacketNumber = 0,
                    ACKNumber = 0,
                    Commands = UnreliableCommandBuffer.ToArray()
                }.Serialize();

                var l2Encrypt = AES.Encrypt(l2, AESKey);

                var packet = new UdpPacket
                {
                    PeerID = UniP2PManager.MyPeerID,
                    UdpPacketL2 = l2Encrypt.result,
                    UdpPacketL2IV = l2Encrypt.iv,
                }.Serialize();

                await UdpSocket.SendAsync(packet, packet.Length, Peer.IPEndPoint);
                PacketCapture.Write(Peer.IPEndPoint.ToString(), packet.Length, "Unreliable");
                UnreliableCommandBuffer.Clear();
            }
        }

        #endregion

        #region Reliable
        
        public async UniTask AddReliableCommand(Command obj)
        {
            ReliableCommandBuffer.Add(obj);
        }

        /// <summary>
        /// Key is Send Number 
        /// </summary>
        private Dictionary<ulong, ReliablePacketInfo> SentReliableBuffer = new Dictionary<ulong, ReliablePacketInfo>();

        public async UniTask SendReliable(UdpClient UdpSocket)
        {
            if (State == UdpConnectionState.Connected)
            {
                /*if (!isTickReliablePacket)
                {
                    isTickReliablePacket = true;
                    await TickReliablePacket(UdpSocket);
                }*/

                if (ReliableCommandBuffer.Count != 0)
                {
                    var l2 = new UdpPacketL2
                    {
                        PacketNumber = MyPacketCount,
                        ACKNumber = 0,
                        Commands = ReliableCommandBuffer.ToArray(),
                    }.Serialize();

                    var l2Encrypt = AES.Encrypt(l2, AESKey);

                    var packet = new UdpPacket
                    {
                        PeerID = UniP2PManager.MyPeerID,
                        UdpPacketL2 = l2Encrypt.result,
                        UdpPacketL2IV = l2Encrypt.iv,
                    }.Serialize();

                    await UdpSocket.SendAsync(packet, packet.Length, Peer.IPEndPoint);
                    PacketCapture.Write(Peer.IPEndPoint.ToString(), packet.Length, "Reliable");
                    //SentReliableBuffer.Add(MyPacketCount, new ReliablePacketInfo { Buffer = packet, WaitTime = 0 });
                    MyPacketCount++;
                }

                /*if (SentReliableBuffer.ContainsKey(PeerWaitPacketNumber))
                {
                    await UdpSocket.SendAsync(SentReliableBuffer[PeerWaitPacketNumber].Buffer, SentReliableBuffer[PeerWaitPacketNumber].Buffer.Length, Peer.IPEndPoint);
                    PacketCapture.Write(Peer.IPEndPoint.ToString(), SentReliableBuffer[PeerWaitPacketNumber].Buffer.Length, "SentReliableBuffer");
                }*/
            }
        }

        public async UniTask TickReliablePacket(UdpClient UdpSocket)
        {
            while (isTickReliablePacket)
            {
                foreach (var value in SentReliableBuffer.Values)
                {
                    value.WaitTime++;
                    if (RTT <= value.WaitTime)
                    {
                        await UdpSocket.SendAsync(value.Buffer, value.Buffer.Length, Peer.IPEndPoint);
                        PacketCapture.Write(Peer.IPEndPoint.ToString(), value.Buffer.Length, "TickReliablePacket");
                    }
                }

                await UniTask.Delay(1);
            }
        }

        public void ReceiveACKPacket(ulong ack)
        {
            PeerWaitPacketNumber = ack + 1;
            SentReliableBuffer.Remove(ack);
        }

        public void Dispose()
        {
            DisConnect();
        }

        #endregion

    }
}

public class ReliablePacketInfo
{
    public byte[] Buffer;
    public short WaitTime;
}