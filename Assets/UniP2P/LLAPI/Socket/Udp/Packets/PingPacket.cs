using MessagePack;

namespace UniP2P.LLAPI
{
    [MessagePackObject]
    public class PingPacket
    {
        [Key(0)]
        public int PingID;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static PingPacket Deserialize(byte[] value)
        {
            return MessagePackSerializer.Deserialize<PingPacket>(value);
        }
    }
}