using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UniP2P.LLAPI;
using UniRx.Async;
using UnityEngine;

namespace UniP2P.HLAPI
{
    [RequireComponent(typeof(SyncGameObject))]
    public class SyncRigidbody : MonoBehaviour , ISyncReceiverByteArray
    {
        private SyncGameObject SyncGameObject;
        private Rigidbody Rigidbody;

        void Awake()
        {
            SyncGameObject = GetComponent<SyncGameObject>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        async void FixedUpdate()
        {
            await SendRigidbodyAsync();
        }

        private async UniTask SendRigidbodyAsync()
        {
            var packet = new RigidbodySyncPacket();
            packet.Postion = Rigidbody.position;
            packet.Rotation = Rigidbody.rotation;
            packet.Velocity = Rigidbody.velocity;

            await SyncGameObject.SendAsync(packet, typeof(SyncRigidbody));
        }

        public virtual void OnReceiveByteArray(byte[] value, Peer peer)
        {
            if (!SyncGameObject.IsMine)
            {
                var packet = Serializer.Deserialize<RigidbodySyncPacket>(value);
                
                Rigidbody.position = packet.Postion;
                Rigidbody.rotation = packet.Rotation;
                Rigidbody.velocity = packet.Velocity;
            }
        }
    }

    [MessagePackObject]
    public class RigidbodySyncPacket
    {
        [Key(0)]
        public Vector3 Postion;
        [Key(1)]
        public Quaternion Rotation;
        [Key(2)]
        public Vector3 Velocity;
    }
}
