using System;
using UniP2P.LLAPI;

namespace UniP2P.HLAPI
{
    [Serializable]
    public class JoinRandomRoomRequest : IGetHashPlaintext
    {
        public string peerid;
        public string ip;
        public int localport;
        public string encrypt;
        public string metadata;
        public string hash;

        public string GetText()
        {
            return peerid + ip + localport + encrypt + metadata;
        }

        public void SetHash()
        {
            hash = HMAC.GenerateSHA256(GetText(), UniP2PManager.MatchingSettings.MatchingSecretKey);
        }
    }
}
