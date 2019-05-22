using System;
using UniP2P.LLAPI;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class CloseRoomRequest : IGetHashPlaintext
    {
        public string peerid;
        public string roomid;
        public string token;
        public string hash;

        public string GetText()
        {
            return peerid + roomid + token;
        }

        public void SetHash()
        {
            hash = HMAC.GenerateSHA256(GetText(), UniP2PManager.MatchingSettings.MatchingSecretKey);
        }
    }
}
