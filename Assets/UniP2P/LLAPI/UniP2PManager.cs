using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UniP2P.LLAPI;
using UniP2P.Debug;
using UniP2P.HLAPI;
using UniRx.Async;

namespace UniP2P
{
    public static class UniP2PManager
    {
        public const string Version = "0.2.0";

        public static ISocket Socket;

        public static MatchingSettings MatchingSettings;

        public static AdvancedSettings AdvancedSettings;

        public const string DefaultMatchingSettingsPath = "Assets/UniP2P/Resources/MatchingSettings.asset";

        public const string DefaultAdvancedSettingsPath = "Assets/UniP2P/Resources/AdvancedSettings.asset";

        public const string MatchingSettingsFileName = "MatchingSettings";

        public const string AdvancedSettingsFileName = "AdvancedSettings";

        public const string SettingFileExtension = ".asset";

        public static IPEndPoint PrivateIPEndPoint = new IPEndPoint(0,0);

        public static IPEndPoint StunIPEndPoint = new IPEndPoint(0,0);

        public static bool isGetStunResult;

        private static List<Peer> Peers = new List<Peer>();

        public static string MyPeerID
        {
            get;set;
        }

        private static bool isInit;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnLoadInit()
        {
            if (Resources.Load<AdvancedSettings>(AdvancedSettingsFileName).AutoOnLoadInit)
            {
                CheckInit();
            }
        }

        public static void CheckInit()
        {
            if (!isInit)
            {
                SettingLoad();
                Init();
            }
        }

        private static bool isLoaded;
        public static void SettingLoad()
        {
            if (!isLoaded)
            {
                MatchingSettings = Resources.Load<MatchingSettings>(MatchingSettingsFileName);
                AdvancedSettings = Resources.Load<AdvancedSettings>(AdvancedSettingsFileName);
                foreach (var console in AdvancedSettings.Consoles)
                {
                    switch (console)
                    {
                        case ConsoleType.System:
                            Debugger.SetConsole(new SystemConsole());
                            break;
                        case ConsoleType.Unity:
                            Debugger.SetConsole(new UnityConsole());
                            break;
                        case ConsoleType.Extension:
                            Debugger.SetConsole(new ExConsole());
                            break;
                    }
                }
               
                switch (AdvancedSettings.SocketType)
                {
                    case SocketType.dotnetUDP:
                        Socket = new SocketUdp();
                        break;
                }

                isLoaded = true;
                Debugger.Log("[UniP2PManager] SettingLoaded");
            }
        }

        public static void Init()
        {
            RandomMyPeerID();
            Socket.Init();
            GetPrivateIPEndPoint();
            if (AdvancedSettings.isSTUN)
            {
                StunIPEndPoint = Socket.GetSTUNResult();
                isGetStunResult = true;
            }
            ManagersInit();
            isInit = true;
            Debugger.Log("[UniP2PManager] Initialized");
        }
        
        private static void GetPrivateIPEndPoint()
        {
            string localIP;
            using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse(localIP), Socket.BindPort());
            Debugger.Log("[UniP2PManager] Private IPEndPoint:" + PrivateIPEndPoint);
        }

        public static IPEndPoint GetEnableIPEndPoint()
        {
            if (isGetStunResult)
            {
                return StunIPEndPoint;
            }
            else
            {
                return PrivateIPEndPoint;
            }
        }

        public static void Shutdown()
        {
            Socket.Shutdown();
        }
        
        private static void RandomMyPeerID()
        {
            MyPeerID = Guid.NewGuid().ToString("N");
            Debugger.Log("[UniP2PManager] Generate MyPeerID:" + MyPeerID);
        }

        private static void ManagersInit()
        {
            DataEventManager.Init();
        }

        public static async UniTask<Peer> ConnectPeerAsync(string address, int port, string peerid = "", int localport = 0)
        {
            return await ConnectPeerAsync(new IPEndPoint(IPAddress.Parse(address), port), peerid, localport);          
        }

        public static async UniTask<Peer> ConnectPeerAsync(IPEndPoint ip, string peerid = "", int localport = 0)
        {
            foreach (var p in Peers)
            {
                if (Equals(p.IPEndPoint,ip) || p.ID == peerid)
                {
                    Debugger.Error("[UniP2PManager] Already Connected.");
                    return null;
                }
            }
            
            if (Equals(ip, PrivateIPEndPoint) || Equals(ip, StunIPEndPoint))
            {
                Debugger.Error("[UniP2PManager] IPEndPoint is MySelf. :" + ip);
                return null; 
            }
            var peer = await Socket.ConnectPeerAsync(ip,peerid,localport);
            if (peer != null)
            {
                Debugger.Log("[UniP2PManager] Connected Peer id:" + peer.ID + " IPEndPoint:" + peer.IPEndPoint);
            }
            return peer;
        }
    
        public static async UniTask DisConnectPeerAsync(Peer peer)
        {
            await Socket.DisConnectAsync(peer);
            RemovePeer(peer);
        }

        public static async UniTask DisConnectAllPeerAsync()
        {
            foreach (Peer p in Peers.ToArray())
            {
                await DisConnectPeerAsync(p);
            }
        }

        public static async UniTask SendPacketAsync(Peer peer, byte[] data , CommandType eventtype, SocketQosType qostype)
        {
            await Socket.SendPacketAsync(peer, data, eventtype , qostype);
        }

        public static async UniTask SendPacketAllPeerAsync(byte[] data, CommandType eventtype, SocketQosType qostype)
        {
            foreach (Peer p in Peers)
            {
                await SendPacketAsync(p, data, eventtype, qostype);
            }
        }

        public static Peer GetPeer(string peerid)
        {
            foreach (Peer p in Peers)
            {
                if (p.ID == peerid)
                {
                    return p;
                }
            }
            return null;
        }

        public static Peer GetConnectedPeer(string peerid)
        {
            foreach (Peer p in Peers)
            {
                if (p.ID == peerid && p.State == PeerState.Connected)
                {
                    return p;
                }
            }
            return null;
        }

        public static Peer[] GetAllPeer()
        {
            return Peers.ToArray();
        }

        public static List<string> GetAllPeersID()
        {
            List<string> peersid = new List<string>();
            foreach(var peer in GetAllPeer())
            {
                peersid.Add(peer.ID);
            }
            return peersid;
        }

        public static int GetMyPeerOrder()
        {
            var peers = GetAllPeersID();
            peers.Add(MyPeerID);
            peers.Sort();
            int i = 0;
            foreach (var peer in peers)
            {
                if (peer == MyPeerID)
                {
                    return i;
                }
                i++;
            }
            Debugger.Error("Failed Sort.");
            return -1;
        }

        public static int GetPeerConnectedCount()
        {
            int i = 0;
            foreach (var peer in Peers)
            {
                if (peer.State == PeerState.Connected)
                {
                    i++;
                }
            }
            return i;
        }

        public static int GetPeerRequestingCount()
        {
            int i = 0;
            foreach (var peer in Peers)
            {
                if (peer.State == PeerState.Connecting)
                {
                    i++;
                }
            }
            return i;
        }

        public static int GetPeerCount()
        {
            return Peers.ToArray().Length;
        }

        public static void AddPeer(Peer peer)
        {
            Peers.Add(peer);
        }

        public static void RemovePeer(Peer peer)
        {
            Peers.Remove(peer);
        }

        public static async UniTask SendEmptyPacketAsync(IPEndPoint ip)
        {
            await Socket.SendEmptyPacketAsync(ip);
        }

        public static UnityEngine.Object Instantiate(string resourcepath)
        {
            return DataEventManager.Instantiate(resourcepath, Vector3.zero, Quaternion.identity).Result;
        }

        public static UnityEngine.Object Instantiate(string resourcepath, Vector3 position, Quaternion quaternion)
        {
            return DataEventManager.Instantiate(resourcepath, position, quaternion).Result;
        }

        public static async void Destroy(UnityEngine.Object obj)
        {
            await DataEventManager.Destroy(obj);
        }

        public static async UniTask<UnityEngine.Object> InstantiateAsync(string resourcepath)
        {
            return await DataEventManager.Instantiate(resourcepath, Vector3.zero, Quaternion.identity);
        }

        public static async UniTask<UnityEngine.Object> InstantiateAsync(string resourcepath, Vector3 position, Quaternion quaternion)
        {
            return await DataEventManager.Instantiate(resourcepath, Vector3.zero, Quaternion.identity);
        }

        public static async UniTask DestoryAsync(UnityEngine.Object obj)
        {
            await DataEventManager.Destroy(obj);
        }
        
        public static async UniTask<int> Ping(Peer peer)
        {
            return await Ping(peer.IPEndPoint);
        }

        public static async UniTask<int> Ping(IPEndPoint ip)
        {
            return await Socket.SendPingPacketAsync(ip);
        }
    }

}
