using System.Collections.Generic;
using MessagePack;

namespace UniP2P.HLAPI
{
    [MessagePackObject]
    public class MatchingLANPacket
    {
        [Key(0)]
        public LANBroadcastEvent Event;
        [Key(1)]
        public string GameVersion;
        [Key(2)]
        public string GameKey;
        [Key(3)]
        public string RoomName;
        [Key(4)]
        public string Peerid;
        [Key(5)]
        public int MaxMember;
        [Key(6)]
        public List<string> PeerIPEndPoints = new List<string>();
    }

    public enum LANBroadcastEvent
    {
        Request,
        Response,
        Start
    }
}
