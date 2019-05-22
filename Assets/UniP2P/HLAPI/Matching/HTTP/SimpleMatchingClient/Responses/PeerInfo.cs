using System;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class PeerInfo
    {
        public string id;
        public string ip;
        public long localport;
        public string encrypt;
        public string metadata;
    }
}