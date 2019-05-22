using System;
using UniP2P.LLAPI;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class CreateRoomRequest : IGetHashPlaintext
    {
        public string peerid;
        public string roomname;
        public int maxmember;
        public string metadata;
        public string hash;

        public string GetText()
        {
            return peerid + roomname + maxmember.ToString() + metadata;
        }

        public void SetHash()
        {
            hash = HMAC.GenerateSHA256(GetText(), UniP2PManager.MatchingSettings.MatchingSecretKey);
        }
    }
}
