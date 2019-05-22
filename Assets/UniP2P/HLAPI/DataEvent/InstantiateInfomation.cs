using MessagePack;
using UnityEngine;

namespace UniP2P.HLAPI
{
    [MessagePackObject]
    public struct InstantiateInfomation
    {
        [Key(0)]
        public Vector3 Position;

        [Key(1)]
        public Quaternion Quaternion;

        [Key(2)]
        public string ResourcePath;
    }
}
