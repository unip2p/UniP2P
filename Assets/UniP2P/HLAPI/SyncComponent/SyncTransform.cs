using MessagePack;
using UniP2P.LLAPI;
using UniRx.Async;
using UnityEngine;

namespace UniP2P.HLAPI
{
    [RequireComponent(typeof(SyncGameObject))]
    public class SyncTransform: MonoBehaviour , ISyncReceiverByteArray
    {
        public bool isSyncPostion;
        public bool isSyncRotation;
        public bool isSyncScale;
        public float SyncPrecisionPostion = 0.001f;
        public float SyncPrecisionRotation = 0.001f;
        public float SyncPrecisionScale = 0.001f;

        private Vector3 CachePostion;
        private Quaternion CacheRotation;
        private Vector3 CacheScale;
        private SyncGameObject SyncGameObject;

        protected virtual void Awake()
        {
            SyncGameObject = GetComponent<SyncGameObject>();
        }

        protected virtual async void Update()
        {
            if (Mathf.Abs(transform.position.x - CachePostion.x) >= SyncPrecisionPostion ||
                Mathf.Abs(transform.position.y - CachePostion.y) >= SyncPrecisionPostion ||
                Mathf.Abs(transform.position.y - CachePostion.z) >= SyncPrecisionPostion && isSyncPostion)
            {
                await SendTransformAsync();
            }
            else if (Mathf.Abs(transform.rotation.x - CacheRotation.x) >= SyncPrecisionRotation ||
                 Mathf.Abs(transform.rotation.y - CacheRotation.y) >= SyncPrecisionRotation ||
                 Mathf.Abs(transform.rotation.y - CacheRotation.z) >= SyncPrecisionRotation && isSyncRotation)
            {
                await SendTransformAsync();
            }
            else if (Mathf.Abs(transform.localScale.x - CacheScale.x) >= SyncPrecisionScale ||
                 Mathf.Abs(transform.localScale.y - CacheScale.y) >= SyncPrecisionScale ||
                 Mathf.Abs(transform.localScale.y - CacheScale.z) >= SyncPrecisionScale && isSyncScale)
            {
                await SendTransformAsync();
            }
        }

        private async UniTask SendTransformAsync()
        {
            var packet = new TransformSyncPacket();

            if (isSyncPostion)
            {
                packet.isSyncPostion = isSyncPostion;
                packet.Postion = transform.position;
                CachePostion = transform.position;
            }

            if (isSyncRotation)
            {
                packet.isSyncRotation = isSyncRotation;
                packet.Rotation = transform.rotation;
                CacheRotation = transform.rotation;
            }

            if (isSyncScale)
            {
                packet.isSyncScale = isSyncScale;
                packet.Scale = transform.localScale;
                CacheScale = transform.localScale;
            }

            if (isSyncPostion || isSyncRotation || isSyncScale && SyncGameObject.IsMine)
            {
                await SyncGameObject.SendAsync(packet, typeof(SyncTransform));
            }
        }

        public virtual void OnReceiveByteArray(byte[] value, Peer peer)
        {
            if (!SyncGameObject.IsMine)
            {
                var packet = Serializer.Deserialize<TransformSyncPacket>(value);
                if (packet.isSyncPostion && isSyncPostion)
                {
                    transform.position = packet.Postion;
                }
                if (packet.isSyncRotation && isSyncRotation)
                {
                    transform.rotation = packet.Rotation;
                }
                if (packet.isSyncScale && isSyncScale)
                {
                    transform.localScale = packet.Scale;
                }
            }        
        }
    }

    [MessagePackObject]
    public class TransformSyncPacket
    {
        [Key(0)]
        public bool isSyncPostion;
        [Key(1)]
        public bool isSyncRotation;
        [Key(2)]
        public bool isSyncScale;
        [Key(3)]
        public Vector3 Postion;
        [Key(4)]
        public Quaternion Rotation;
        [Key(5)]
        public Vector3 Scale;
    }
}
