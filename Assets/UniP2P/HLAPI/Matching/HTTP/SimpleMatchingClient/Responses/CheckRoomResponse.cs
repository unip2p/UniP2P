using System;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class CheckRoomResponse
    {
        public PeerInfo[] peers;
        public bool isclose;
    }
}
