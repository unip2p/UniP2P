#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(17)
            {
                {typeof(global::UniP2P.LLAPI.UdpPacketObject[]), 0 },
                {typeof(global::MessagePack.MessagePackType), 1 },
                {typeof(global::UniP2P.Debug.ConsoleType), 2 },
                {typeof(global::UniP2P.SerializerType), 3 },
                {typeof(global::UniP2P.LLAPI.P2PEventType), 4 },
                {typeof(global::UniP2P.LLAPI.SocketQosType), 5 },
                {typeof(global::UniP2P.LLAPI.SocketType), 6 },
                {typeof(global::UniP2P.LLAPI.UdpConnectionState), 7 },
                {typeof(global::LumiSoft.Net.STUN.Client.STUN_NetType), 8 },
                {typeof(global::LumiSoft.Net.STUN.Message.STUN_MessageType), 9 },
                {typeof(global::SampleCubeControllerPacket), 10 },
                {typeof(global::UniP2P.HLAPI.StreamPacketInstance), 11 },
                {typeof(global::UniP2P.HLAPI.StreamPacket), 12 },
                {typeof(global::UniP2P.LLAPI.ConnectEventByte), 13 },
                {typeof(global::UniP2P.LLAPI.UdpPacketObject), 14 },
                {typeof(global::UniP2P.LLAPI.UdpPacketObjects), 15 },
                {typeof(global::UniP2P.LLAPI.UdpPacketACK), 16 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ArrayFormatter<global::UniP2P.LLAPI.UdpPacketObject>();
                case 1: return new MessagePack.Formatters.MessagePack.MessagePackTypeFormatter();
                case 2: return new MessagePack.Formatters.UniP2P.Debug.ConsoleTypeFormatter();
                case 3: return new MessagePack.Formatters.UniP2P.SerializerTypeFormatter();
                case 4: return new MessagePack.Formatters.UniP2P.LLAPI.P2PEventTypeFormatter();
                case 5: return new MessagePack.Formatters.UniP2P.LLAPI.SocketQosTypeFormatter();
                case 6: return new MessagePack.Formatters.UniP2P.LLAPI.SocketTypeFormatter();
                case 7: return new MessagePack.Formatters.UniP2P.LLAPI.UdpConnectionStateFormatter();
                case 8: return new MessagePack.Formatters.LumiSoft.Net.STUN.Client.STUN_NetTypeFormatter();
                case 9: return new MessagePack.Formatters.LumiSoft.Net.STUN.Message.STUN_MessageTypeFormatter();
                case 10: return new MessagePack.Formatters.SampleCubeControllerPacketFormatter();
                case 11: return new MessagePack.Formatters.UniP2P.HLAPI.StreamPacketInstanceFormatter();
                case 12: return new MessagePack.Formatters.UniP2P.HLAPI.StreamPacketFormatter();
                case 13: return new MessagePack.Formatters.UniP2P.LLAPI.ConnectEventByteFormatter();
                case 14: return new MessagePack.Formatters.UniP2P.LLAPI.UdpPacketObjectFormatter();
                case 15: return new MessagePack.Formatters.UniP2P.LLAPI.UdpPacketObjectsFormatter();
                case 16: return new MessagePack.Formatters.UniP2P.LLAPI.UdpPacketACKFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.MessagePack
{
    using System;
    using MessagePack;

    public sealed class MessagePackTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.MessagePackType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::MessagePack.MessagePackType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::MessagePack.MessagePackType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::MessagePack.MessagePackType)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.UniP2P.Debug
{
    using System;
    using MessagePack;

    public sealed class ConsoleTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.Debug.ConsoleType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.Debug.ConsoleType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.Debug.ConsoleType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.Debug.ConsoleType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.UniP2P
{
    using System;
    using MessagePack;

    public sealed class SerializerTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.SerializerType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.SerializerType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.SerializerType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.SerializerType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.UniP2P.LLAPI
{
    using System;
    using MessagePack;

    public sealed class P2PEventTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.P2PEventType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.P2PEventType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.P2PEventType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.P2PEventType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class SocketQosTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.SocketQosType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.SocketQosType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.SocketQosType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.SocketQosType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class SocketTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.SocketType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.SocketType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.SocketType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.SocketType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class UdpConnectionStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpConnectionState>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpConnectionState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.UdpConnectionState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.UdpConnectionState)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.LumiSoft.Net.STUN.Client
{
    using System;
    using MessagePack;

    public sealed class STUN_NetTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LumiSoft.Net.STUN.Client.STUN_NetType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::LumiSoft.Net.STUN.Client.STUN_NetType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::LumiSoft.Net.STUN.Client.STUN_NetType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::LumiSoft.Net.STUN.Client.STUN_NetType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.LumiSoft.Net.STUN.Message
{
    using System;
    using MessagePack;

    public sealed class STUN_MessageTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LumiSoft.Net.STUN.Message.STUN_MessageType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::LumiSoft.Net.STUN.Message.STUN_MessageType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::LumiSoft.Net.STUN.Message.STUN_MessageType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::LumiSoft.Net.STUN.Message.STUN_MessageType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters
{
    using System;
    using MessagePack;


    public sealed class SampleCubeControllerPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SampleCubeControllerPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::SampleCubeControllerPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.pos, formatterResolver);
            return offset - startOffset;
        }

        public global::SampleCubeControllerPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __pos__ = default(global::UnityEngine.Vector3);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __pos__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::SampleCubeControllerPacket();
            ____result.pos = __pos__;
            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.UniP2P.HLAPI
{
    using System;
    using MessagePack;


    public sealed class StreamPacketInstanceFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.StreamPacketInstance>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.StreamPacketInstance value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Serialize(ref bytes, offset, value.Quaternion, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.NetInstanceID, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.StreamPacketInstance Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Position__ = default(global::UnityEngine.Vector3);
            var __Quaternion__ = default(global::UnityEngine.Quaternion);
            var __NetInstanceID__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Position__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Quaternion__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __NetInstanceID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.StreamPacketInstance();
            ____result.Position = __Position__;
            ____result.Quaternion = __Quaternion__;
            ____result.NetInstanceID = __NetInstanceID__;
            return ____result;
        }
    }


    public sealed class StreamPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.StreamPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.StreamPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 5);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.NetInstanceID, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.OutComponentType, formatterResolver);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isInstantiate);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.InstantiateResourcePath, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.value, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.StreamPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __NetInstanceID__ = default(string);
            var __OutComponentType__ = default(string);
            var __isInstantiate__ = default(bool);
            var __InstantiateResourcePath__ = default(string);
            var __value__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __NetInstanceID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __OutComponentType__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __isInstantiate__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 3:
                        __InstantiateResourcePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __value__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.StreamPacket();
            ____result.NetInstanceID = __NetInstanceID__;
            ____result.OutComponentType = __OutComponentType__;
            ____result.isInstantiate = __isInstantiate__;
            ____result.InstantiateResourcePath = __InstantiateResourcePath__;
            ____result.value = __value__;
            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.UniP2P.LLAPI
{
    using System;
    using MessagePack;


    public sealed class ConnectEventByteFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.ConnectEventByte>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.ConnectEventByte value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PeerID, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.ConnectEventByte Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __PeerID__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __PeerID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.ConnectEventByte();
            ____result.PeerID = __PeerID__;
            return ____result;
        }
    }


    public sealed class UdpPacketObjectFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpPacketObject>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpPacketObject value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.P2PEventType>().Serialize(ref bytes, offset, value.P2PEventType, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Value, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.SocketQosType>().Serialize(ref bytes, offset, value.QosType, formatterResolver);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.sequence_no);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.UdpPacketObject Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __P2PEventType__ = default(global::UniP2P.LLAPI.P2PEventType);
            var __Value__ = default(byte[]);
            var __QosType__ = default(global::UniP2P.LLAPI.SocketQosType);
            var __sequence_no__ = default(ulong);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __P2PEventType__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.P2PEventType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __QosType__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.SocketQosType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __sequence_no__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.UdpPacketObject();
            ____result.P2PEventType = __P2PEventType__;
            ____result.Value = __Value__;
            ____result.QosType = __QosType__;
            ____result.sequence_no = __sequence_no__;
            return ____result;
        }
    }


    public sealed class UdpPacketObjectsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpPacketObjects>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpPacketObjects value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.UdpPacketObject[]>().Serialize(ref bytes, offset, value.UdpPacketObjectArray, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PeerID, formatterResolver);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.ack_no);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.UdpPacketObjects Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __UdpPacketObjectArray__ = default(global::UniP2P.LLAPI.UdpPacketObject[]);
            var __PeerID__ = default(string);
            var __ack_no__ = default(ulong);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __UdpPacketObjectArray__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.UdpPacketObject[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __PeerID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __ack_no__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.UdpPacketObjects();
            ____result.UdpPacketObjectArray = __UdpPacketObjectArray__;
            ____result.PeerID = __PeerID__;
            ____result.ack_no = __ack_no__;
            return ____result;
        }
    }


    public sealed class UdpPacketACKFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpPacketACK>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpPacketACK value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.packetid);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.UdpPacketACK Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __packetid__ = default(ulong);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __packetid__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.UdpPacketACK();
            ____result.packetid = __packetid__;
            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
