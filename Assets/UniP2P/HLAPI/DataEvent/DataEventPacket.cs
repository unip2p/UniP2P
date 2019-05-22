using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using UnityEngine;

namespace UniP2P.HLAPI
{
    [MessagePackObject]
    public struct DataEventPacket
    {
        [IgnoreMember]
        public const string Instantiate = "Instantiate";

        [IgnoreMember]
        public const string Destroy = "Destory";

        [IgnoreMember]
        public const string RPC = "RPC";

        [IgnoreMember]
        public const string Data = "Data";

        [Key(0)]
        public string InstanceID;

        [Key(1)] 
        public string TypeName;

        [Key(2)]
        public string EventName;

        [Key(3)]
        public byte[] Value;

    }
}
