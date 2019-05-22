using MessagePack;
using System.Collections.Generic;
using UniP2P.LLAPI;
using UniP2P.Debug;
using UniRx;
using UnityEngine;
using UniRx.Async;

namespace UniP2P.HLAPI
{
    public static class DataEventManager
    {
        public static bool IsDataEventQueueing
        {
            get
            {
                return isDataEventQueueing;
            }
            set
            {
                isDataEventQueueing = value;
                if (value == false)
                {
                    CheckDataEventQueue();
                }
            }     
        }

        private static bool isDataEventQueueing;

        public static Queue<P2PEventArgs> DataEventQueue = new Queue<P2PEventArgs>();

        private static Dictionary<string, SyncGameObject> ListenSyncGameObjects = new Dictionary<string, SyncGameObject>();

        private static List<IInstantiatedListener> InstantiatedListeners = new List<IInstantiatedListener>();
        
        private static bool isInit = false;
        public static void Init()
        {
            if (!isInit)
            {
                UniP2PManager.Socket.OnReceiveDataChanged().Subscribe(OnDataReceived);
                isInit = true;
                Debugger.Log("[DataEventManager] Initialized");
            }
        }

        public static void ListenSyncGameObject(SyncGameObject sync)
        {
            if (!ListenSyncGameObjects.ContainsKey(sync.InstanceID))
            {
                ListenSyncGameObjects.Add(sync.InstanceID, sync);
            }
            else
            {
                Debugger.Warning("[DataEventManager] Already Listen SyncGameObject Instance:" + sync.InstanceID);
            }
        }
        
        public static void ListenInstantiated(IInstantiatedListener listener)
        {
            if (!InstantiatedListeners.Contains(listener))
            {
                InstantiatedListeners.Add(listener);
            }
            else
            {
                Debugger.Warning("[DataEventManager] Already Listen InstantiatedListener.");
            }
        }

        private static void CheckDataEventQueue()
        {
            for (int i = 0; i < DataEventQueue.Count; i++)
            {
                var args = DataEventQueue.Dequeue();
                OnDataReceived(args);
            }
        }

        private static void OnDataReceived(P2PEventArgs args)
        {
            if (!IsDataEventQueueing)
            {
                switch (args.eventType)
                {
                    case CommandType.DataEvent:
                        var packet = MessagePackSerializer.Deserialize<DataEventPacket>(args.data);
                        switch (packet.EventName)
                        {
                            case DataEventPacket.Instantiate:
                                var info = Serializer.Deserialize<InstantiateInfomation>(packet.Value);
                                var obj = Object.Instantiate(Resources.Load(info.ResourcePath), info.Position, info.Quaternion) as GameObject;
                                if (obj != null)
                                {
                                    var sync = obj.GetComponent<SyncGameObject>();

                                    if (sync == null)
                                    {
                                        Debugger.Error("[DataEventManager] Network Instantiate need SyncGameObject Component. Resources path:" + info.ResourcePath);
                                    }
                                    else
                                    {
                                        sync.Instance(args.peer.ID, packet.InstanceID);
                                        foreach (var listener in InstantiatedListeners)
                                        {
                                            listener.OnInstantiated(obj, info.ResourcePath);
                                        }
                                    }
                                }
                                else
                                {
                                    Debugger.Error("[DataEventManager] Not Found GameObject Resources Folder. Resources path:" + info.ResourcePath);
                                }
                                break;

                            case DataEventPacket.Data:
                                if (ListenSyncGameObjects.ContainsKey(packet.InstanceID))
                                {
                                    ListenSyncGameObjects[packet.InstanceID]
                                        .ReceiveByteArray(packet.Value, packet.TypeName, args.peer);
                                }
                                else
                                {
                                    Debugger.Error("[DataEventManager] Not Found SyncGameObject:" + packet.InstanceID);
                                }
                                break;

                            case DataEventPacket.RPC:
                                if (ListenSyncGameObjects.ContainsKey(packet.InstanceID))
                                {
                                    var sync = ListenSyncGameObjects[packet.InstanceID];
                                    var o = Serializer.Deserialize<RPCRequest>(packet.Value);
                                    sync.ReceiveRPC(o.MethodName, packet.TypeName, o.Parameter);
                                }
                                else
                                {
                                    Debugger.Error("[DataEventManager] Not Found SyncGameObject:" + packet.InstanceID);
                                }
                                break;

                            case DataEventPacket.Destroy:
                                if (ListenSyncGameObjects.ContainsKey(packet.InstanceID))
                                {
                                    Object.Destroy(ListenSyncGameObjects[packet.InstanceID].gameObject);
                                }
                                else
                                {
                                    Debugger.Error("[DataEventManager] Not Found SyncGameObject:" + packet.InstanceID);
                                }
                                break;

                        }
                        break;
                    case CommandType.Disconnect:
                        foreach (var obj in ListenSyncGameObjects.Values)
                        {
                            if (args.peer.ID == obj.InstancePeerid && obj.DisconnectToDestory)
                            {
                                Object.Destroy(obj.gameObject);
                            }
                        }
                        break;
                }
            }
            else
            {
                DataEventQueue.Enqueue(args);
            }
        }
        
        public static async UniTask SendStream(string instanceid, string eventname,string typename, byte[] value, SocketQosType qostype = SocketQosType.Unreliable, string peerid = "")
        {
            var peer = UniP2PManager.GetPeer(peerid);
            if (peer == null)
            {
                return;
            }
            await UniP2PManager.SendPacketAsync(peer, CreatePacket(eventname, value, instanceid, typename), CommandType.DataEvent, qostype);
        }

        public static async UniTask SendStreamAllPeer(string instanceid, string eventname, string typename,byte[] value, SocketQosType qostype = SocketQosType.Unreliable)
        {
            await UniP2PManager.SendPacketAllPeerAsync(CreatePacket(eventname, value, instanceid, typename),
                CommandType.DataEvent, qostype);
        }

        public static async UniTask<Object> Instantiate(string resourcepath, Vector3 position, Quaternion quaternion, SocketQosType qostype = SocketQosType.Unreliable)
        {
            var obj = Object.Instantiate(Resources.Load(resourcepath), position, quaternion);
            var gameObject = (GameObject)obj;
            var id = gameObject.GetComponent<SyncGameObject>().Instance(UniP2PManager.MyPeerID);
            var buf = Serializer.Serialize(new InstantiateInfomation { Position = position, Quaternion = quaternion, ResourcePath = resourcepath });
            await UniP2PManager.SendPacketAllPeerAsync(CreatePacket(DataEventPacket.Instantiate, buf, id), CommandType.DataEvent, qostype);
            return obj;
        }

        public static async UniTask<Object> Instantiate(string resourcepath, Vector3 position, Quaternion quaternion, string peerid, SocketQosType qostype = SocketQosType.Unreliable)
        {
            var sendpeer = UniP2PManager.GetPeer(peerid);
            var obj = Object.Instantiate(Resources.Load(resourcepath), position, quaternion);
            var gobj = (GameObject)obj;
            var id = gobj.GetComponent<SyncGameObject>().Instance(UniP2PManager.MyPeerID);
            var buf = Serializer.Serialize(new InstantiateInfomation { Position = position, Quaternion = quaternion, ResourcePath = resourcepath });
            if (sendpeer == null)
            {
                return obj;
            }
            await UniP2PManager.SendPacketAsync(sendpeer, CreatePacket(DataEventPacket.Instantiate, buf, id), CommandType.DataEvent, qostype);
            return obj;
        }

        public static async UniTask Destroy(Object obj, SocketQosType qostype = SocketQosType.Unreliable)
        {
            var gameObject = (GameObject)obj;
            await UniP2PManager.SendPacketAllPeerAsync(CreatePacket(DataEventPacket.Destroy, null, gameObject.GetComponent<SyncGameObject>().InstanceID), CommandType.DataEvent, qostype);
            Object.Destroy(gameObject);
        }

        public static async UniTask Destroy(Object obj, string peerid, SocketQosType qostype = SocketQosType.Unreliable)
        {
            var peer = UniP2PManager.GetPeer(peerid);
            var gameObject = (GameObject)obj;
            await UniP2PManager.SendPacketAsync(peer,CreatePacket(DataEventPacket.Destroy, null, gameObject.GetComponent<SyncGameObject>().InstanceID), CommandType.DataEvent, qostype);
            Object.Destroy(gameObject);
        }

        private static byte[] CreatePacket(string eventname, byte[] value, string instanceid, string typename = "")
        {
            var packet = new DataEventPacket
            {
                EventName = eventname,
                Value = value,
                InstanceID = instanceid,
                TypeName = typename
            };
            var buf = MessagePackSerializer.Serialize(packet);
            return buf;
        }

        public static void ClearList()
        {
            ListenSyncGameObjects.Clear();
        }
    }
}
