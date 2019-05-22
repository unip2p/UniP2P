using MessagePack;
using System;

namespace UniP2P.LLAPI
{
    [MessagePackObject]
    public class UdpPacketL2
    {
        [Key(0)]
        public ulong PacketNumber;
        [Key(1)]
        public ulong ACKNumber;
        [Key(2)]
        public Command[] Commands;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static UdpPacketL2 Deserialize(byte[] value)
        {
            return MessagePackSerializer.Deserialize<UdpPacketL2>(value);
        }
    }

}
