using System;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class JoinRandomRoomResponse
    {
        public string roomid;
        public PeerInfo[] peers;
        public string token;
        public string metadata;
    }

}
