using MessagePack;

namespace UniP2P.LLAPI
{
    [MessagePackObject]
    public class UdpPacket
    {
        [Key(0)]
        public string PeerID;
        [Key(1)]
        public byte[] UdpPacketL2;
        [Key(2)]
        public byte[] UdpPacketL2IV;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static UdpPacket Deserialize(byte[] value)
        {
            return MessagePackSerializer.Deserialize<UdpPacket>(value);
        }

        public bool isEncrypt()
        {
            return UdpPacketL2IV != null;
        }
    }

}
