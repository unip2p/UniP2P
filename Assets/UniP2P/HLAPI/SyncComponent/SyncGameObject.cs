using System;
using System.Collections.Generic;
using System.Reflection;
using UniP2P.LLAPI;
using UniRx.Async;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniP2P.HLAPI
{
    public class SyncGameObject : MonoBehaviour
    {
        private List<ISyncReceiverByteArray> ListISyncReceiver = new List<ISyncReceiverByteArray>();
        
        [ReadOnly] 
        public string InstancePeerid;
        
        [ReadOnly]
        public string InstanceID;       
        
        public bool DisconnectToDestory = true;

        public bool isDynamic
        {
            get { return InstancePeerid != ""; }
        }

        public bool IsMine => InstancePeerid == UniP2PManager.MyPeerID;


        public void Reset()
        {
            InstanceID = gameObject.GetInstanceID().ToString();
        }

        public string Instance(string peerid, string id = "")
        {
            InstancePeerid = peerid;

            if (id == "")
            {
                InstanceID = Random.Range(int.MinValue, int.MaxValue).ToString();
            }
            else
            {
                InstanceID = id;
            }

            Listen();
            DataEventManager.ListenSyncGameObject(this);
            OnInstantiated();
            return InstanceID;
        }

        void Awake()
        {
            if (!isDynamic)
            {
                Listen();
            }
        }

        public void Listen()
        {
            DataEventManager.ListenSyncGameObject(this);
            ListISyncReceiver.Clear();
            foreach (var receiver in GetComponents<ISyncReceiverByteArray>())
            {
                ListISyncReceiver.Add(receiver);
            }
        }

        public async UniTask SendAsync<T>(T obj, Type type, Peer peer = null, bool ismine = false)
        {
            byte[] packet = Serializer.SerializePublicField(obj);
            if (ismine)
            {
                ReceiveByteArray(packet, type.FullName, null);
            }

            await SendByteArrayAsync(packet, type, peer);
        }

        public async UniTask SendByteArrayAsync(byte[] packet, Type type, Peer peer = null)
        {
            var typename = type.FullName;
            if (peer == null)
            {
                await DataEventManager.SendStreamAllPeer(InstanceID, DataEventPacket.Data, typename, packet);
            }
            else
            {
                await DataEventManager.SendStream(InstanceID,  DataEventPacket.Data, typename, packet,
                    SocketQosType.Unreliable, peer.ID);
            }
        }

        public void ReceiveByteArray(byte[] value, string typename, Peer peer)
        {
            var type = Type.GetType(typename);
            if (type != null)
            {
                if (gameObject != null)
                {
                    var component = GetComponent(typename);
                    if (component != null)
                    {
                        var method = typeof(ISyncReceiverByteArray).GetMethod(nameof(ISyncReceiverByteArray.OnReceiveByteArray));
                        if (method != null)
                        {
                            method.Invoke(component, new object[] {value, peer});
                        }
                    }
                }
            }
        }

        #region RPC

        public async UniTask RPC(Action action, Type type, bool isMyInclude = true, Peer peer = null)
        {
            if (isMyInclude)
            {
                ReceiveRPC(action.Method.Name, type.FullName , null);
            }

            await SendRPC(action.Method.Name, type.FullName, null, peer);
        }

        public async UniTask RPC(string action, Type type, object parameter, bool isMyInclude = true, Peer peer = null)
        {
            await RPC(action, type, new object[] { parameter }, isMyInclude, peer);
        }

        public async UniTask RPC(string action, Type type, object[] parameters, bool isMyInclude = true, Peer peer = null)
        {
            var para = Serializer.SerializePublicField(parameters);
            if (isMyInclude)
            {
                ReceiveRPC(action, type.FullName , para);
            }

            await SendRPC(action, type.FullName, para, peer);
        }


        public void ReceiveRPC(string methodName, string typename , byte[] parameterBytes)
        {
            var type = Type.GetType(typename);
            if (type != null)
            {
                var method = type.GetMethod(methodName);
                
                if (method != null)
                {
                    if (!method.IsPublic)
                    {
                        Debug.Debugger.Error("[ReceiveRPC] Cannot Invoke. Method needs to public.");
                    }
                    if (method.IsDefined(typeof(P2PRPCAttribute)))
                    {
                        var obj = GetComponent(type);
                        if (obj != null)
                        {
                            object[] parameter = null;
                            if (parameterBytes != null)
                            {
                                parameter = Serializer.DeserializePublicField<object[]>(parameterBytes);
                            }

                            method.Invoke(obj, parameter);
                        }
                        else
                        {
                            Debug.Debugger.Error("[ReceiveRPC] Not Found Component.");
                        }
                    }
                    else
                    {
                        Debug.Debugger.Error("[ReceiveRPC] Not Defined P2PRPCAttribute.");
                    }
                }
                else
                {
                    Debug.Debugger.Error("[ReceiveRPC] Not Found Method.");
                }
            }
            else
            {
                Debug.Debugger.Error("[ReceiveRPC] Not Found Type.");
            }
        }

        private async UniTask SendRPC(string methodName, string typename, byte[] para, Peer peer = null)
        {
            if (peer == null)
            {
                await DataEventManager.SendStreamAllPeer(InstanceID, DataEventPacket.RPC, typename
                    , Serializer.Serialize(new RPCRequest(methodName, para)));
            }
            else
            {
                await DataEventManager.SendStream(InstanceID, DataEventPacket.RPC, typename
                    , Serializer.Serialize(new RPCRequest(methodName, para))
                    , SocketQosType.Unreliable, peer.ID);
            }
        }

        #endregion

        #region Event Function

        public virtual void OnInstantiated() { }

        #endregion
    }
}