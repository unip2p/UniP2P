using System;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class Room
    {
        public string roomid;
        public string roomname;
        public long maxmember;
        public long currentmember;
        public string metadata;
    }
}
