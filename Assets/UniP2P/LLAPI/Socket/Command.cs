using MessagePack;

namespace UniP2P.LLAPI
{
    [MessagePackObject]
    public class Command
    {
        [Key(0)]
        public CommandType P2PEventType;
        [Key(1)]
        public byte[] Value;
    }
}
