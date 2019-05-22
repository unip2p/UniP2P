using System;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class JoinRoomResponse
    {
        public string roomid;
        public PeerInfo[] peers;
        public string token;
        public string metadata;
    }

}
