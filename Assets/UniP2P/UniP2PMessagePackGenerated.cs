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
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(25)
            {
                {typeof(global::System.Collections.Generic.List<string>), 0 },
                {typeof(global::UniP2P.LLAPI.Command[]), 1 },
                {typeof(global::MessagePack.MessagePackType), 2 },
                {typeof(global::UniP2P.Debug.ConsoleType), 3 },
                {typeof(global::UniP2P.Debug.TypeDebugger), 4 },
                {typeof(global::UniP2P.HLAPI.LANBroadcastEvent), 5 },
                {typeof(global::UniP2P.LLAPI.PeerState), 6 },
                {typeof(global::UniP2P.SerializerType), 7 },
                {typeof(global::UniP2P.LLAPI.CommandType), 8 },
                {typeof(global::UniP2P.LLAPI.SocketQosType), 9 },
                {typeof(global::UniP2P.LLAPI.SocketType), 10 },
                {typeof(global::UniP2P.LLAPI.UdpConnectionState), 11 },
                {typeof(global::LumiSoft.Net.STUN.Client.STUN_NetType), 12 },
                {typeof(global::LumiSoft.Net.STUN.Message.STUN_MessageType), 13 },
                {typeof(global::UniP2P.HLAPI.DataEventPacket), 14 },
                {typeof(global::UniP2P.HLAPI.InstantiateInfomation), 15 },
                {typeof(global::UniP2P.HLAPI.MatchingLANPacket), 16 },
                {typeof(global::UniP2P.HLAPI.RPCRequest), 17 },
                {typeof(global::UniP2P.HLAPI.RigidbodySyncPacket), 18 },
                {typeof(global::UniP2P.HLAPI.TransformSyncPacket), 19 },
                {typeof(global::UniP2P.LLAPI.RSAPublicKey), 20 },
                {typeof(global::UniP2P.LLAPI.Command), 21 },
                {typeof(global::UniP2P.LLAPI.PingPacket), 22 },
                {typeof(global::UniP2P.LLAPI.UdpPacket), 23 },
                {typeof(global::UniP2P.LLAPI.UdpPacketL2), 24 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<string>();
                case 1: return new global::MessagePack.Formatters.ArrayFormatter<global::UniP2P.LLAPI.Command>();
                case 2: return new MessagePack.Formatters.MessagePack.MessagePackTypeFormatter();
                case 3: return new MessagePack.Formatters.UniP2P.Debug.ConsoleTypeFormatter();
                case 4: return new MessagePack.Formatters.UniP2P.Debug.TypeDebuggerFormatter();
                case 5: return new MessagePack.Formatters.UniP2P.HLAPI.LANBroadcastEventFormatter();
                case 6: return new MessagePack.Formatters.UniP2P.LLAPI.PeerStateFormatter();
                case 7: return new MessagePack.Formatters.UniP2P.SerializerTypeFormatter();
                case 8: return new MessagePack.Formatters.UniP2P.LLAPI.CommandTypeFormatter();
                case 9: return new MessagePack.Formatters.UniP2P.LLAPI.SocketQosTypeFormatter();
                case 10: return new MessagePack.Formatters.UniP2P.LLAPI.SocketTypeFormatter();
                case 11: return new MessagePack.Formatters.UniP2P.LLAPI.UdpConnectionStateFormatter();
                case 12: return new MessagePack.Formatters.LumiSoft.Net.STUN.Client.STUN_NetTypeFormatter();
                case 13: return new MessagePack.Formatters.LumiSoft.Net.STUN.Message.STUN_MessageTypeFormatter();
                case 14: return new MessagePack.Formatters.UniP2P.HLAPI.DataEventPacketFormatter();
                case 15: return new MessagePack.Formatters.UniP2P.HLAPI.InstantiateInfomationFormatter();
                case 16: return new MessagePack.Formatters.UniP2P.HLAPI.MatchingLANPacketFormatter();
                case 17: return new MessagePack.Formatters.UniP2P.HLAPI.RPCRequestFormatter();
                case 18: return new MessagePack.Formatters.UniP2P.HLAPI.RigidbodySyncPacketFormatter();
                case 19: return new MessagePack.Formatters.UniP2P.HLAPI.TransformSyncPacketFormatter();
                case 20: return new MessagePack.Formatters.UniP2P.LLAPI.RSAPublicKeyFormatter();
                case 21: return new MessagePack.Formatters.UniP2P.LLAPI.CommandFormatter();
                case 22: return new MessagePack.Formatters.UniP2P.LLAPI.PingPacketFormatter();
                case 23: return new MessagePack.Formatters.UniP2P.LLAPI.UdpPacketFormatter();
                case 24: return new MessagePack.Formatters.UniP2P.LLAPI.UdpPacketL2Formatter();
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

    public sealed class TypeDebuggerFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.Debug.TypeDebugger>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.Debug.TypeDebugger value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.Debug.TypeDebugger Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.Debug.TypeDebugger)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

    public sealed class LANBroadcastEventFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.LANBroadcastEvent>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.LANBroadcastEvent value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.HLAPI.LANBroadcastEvent Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.HLAPI.LANBroadcastEvent)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

    public sealed class PeerStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.PeerState>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.PeerState value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.PeerState Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.PeerState)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class CommandTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.CommandType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.CommandType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, (Int32)value);
        }
        
        public global::UniP2P.LLAPI.CommandType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::UniP2P.LLAPI.CommandType)MessagePackBinary.ReadInt32(bytes, offset, out readSize);
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

namespace MessagePack.Formatters.UniP2P.HLAPI
{
    using System;
    using MessagePack;


    public sealed class DataEventPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.DataEventPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.DataEventPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.InstanceID, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.TypeName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.EventName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Value, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.DataEventPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __InstanceID__ = default(string);
            var __TypeName__ = default(string);
            var __EventName__ = default(string);
            var __Value__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __InstanceID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __TypeName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __EventName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Value__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.DataEventPacket();
            ____result.InstanceID = __InstanceID__;
            ____result.TypeName = __TypeName__;
            ____result.EventName = __EventName__;
            ____result.Value = __Value__;
            return ____result;
        }
    }


    public sealed class InstantiateInfomationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.InstantiateInfomation>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.InstantiateInfomation value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Serialize(ref bytes, offset, value.Quaternion, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.ResourcePath, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.InstantiateInfomation Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __ResourcePath__ = default(string);

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
                        __ResourcePath__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.InstantiateInfomation();
            ____result.Position = __Position__;
            ____result.Quaternion = __Quaternion__;
            ____result.ResourcePath = __ResourcePath__;
            return ____result;
        }
    }


    public sealed class MatchingLANPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.MatchingLANPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.MatchingLANPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 7);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.HLAPI.LANBroadcastEvent>().Serialize(ref bytes, offset, value.Event, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.GameVersion, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.GameKey, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.RoomName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Peerid, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.MaxMember);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref bytes, offset, value.PeerIPEndPoints, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.MatchingLANPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Event__ = default(global::UniP2P.HLAPI.LANBroadcastEvent);
            var __GameVersion__ = default(string);
            var __GameKey__ = default(string);
            var __RoomName__ = default(string);
            var __Peerid__ = default(string);
            var __MaxMember__ = default(int);
            var __PeerIPEndPoints__ = default(global::System.Collections.Generic.List<string>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Event__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.HLAPI.LANBroadcastEvent>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __GameVersion__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __GameKey__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __RoomName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __Peerid__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __MaxMember__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 6:
                        __PeerIPEndPoints__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.MatchingLANPacket();
            ____result.Event = __Event__;
            ____result.GameVersion = __GameVersion__;
            ____result.GameKey = __GameKey__;
            ____result.RoomName = __RoomName__;
            ____result.Peerid = __Peerid__;
            ____result.MaxMember = __MaxMember__;
            ____result.PeerIPEndPoints = __PeerIPEndPoints__;
            return ____result;
        }
    }


    public sealed class RPCRequestFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.RPCRequest>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.RPCRequest value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MethodName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Parameter, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.RPCRequest Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MethodName__ = default(string);
            var __Parameter__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __MethodName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Parameter__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.RPCRequest(__MethodName__, __Parameter__);
            return ____result;
        }
    }


    public sealed class RigidbodySyncPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.RigidbodySyncPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.RigidbodySyncPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Postion, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Serialize(ref bytes, offset, value.Rotation, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Velocity, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.RigidbodySyncPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Postion__ = default(global::UnityEngine.Vector3);
            var __Rotation__ = default(global::UnityEngine.Quaternion);
            var __Velocity__ = default(global::UnityEngine.Vector3);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Postion__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Rotation__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Velocity__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.RigidbodySyncPacket();
            ____result.Postion = __Postion__;
            ____result.Rotation = __Rotation__;
            ____result.Velocity = __Velocity__;
            return ____result;
        }
    }


    public sealed class TransformSyncPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.HLAPI.TransformSyncPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.HLAPI.TransformSyncPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 6);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isSyncPostion);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isSyncRotation);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.isSyncScale);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Postion, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Serialize(ref bytes, offset, value.Rotation, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, offset, value.Scale, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.HLAPI.TransformSyncPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __isSyncPostion__ = default(bool);
            var __isSyncRotation__ = default(bool);
            var __isSyncScale__ = default(bool);
            var __Postion__ = default(global::UnityEngine.Vector3);
            var __Rotation__ = default(global::UnityEngine.Quaternion);
            var __Scale__ = default(global::UnityEngine.Vector3);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __isSyncPostion__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 1:
                        __isSyncRotation__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 2:
                        __isSyncScale__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 3:
                        __Postion__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __Rotation__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Quaternion>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __Scale__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.HLAPI.TransformSyncPacket();
            ____result.isSyncPostion = __isSyncPostion__;
            ____result.isSyncRotation = __isSyncRotation__;
            ____result.isSyncScale = __isSyncScale__;
            ____result.Postion = __Postion__;
            ____result.Rotation = __Rotation__;
            ____result.Scale = __Scale__;
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


    public sealed class RSAPublicKeyFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.RSAPublicKey>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.RSAPublicKey value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Modules, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Exponent, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.RSAPublicKey Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Modules__ = default(byte[]);
            var __Exponent__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Modules__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Exponent__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.RSAPublicKey();
            ____result.Modules = __Modules__;
            ____result.Exponent = __Exponent__;
            return ____result;
        }
    }


    public sealed class CommandFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.Command>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.Command value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.CommandType>().Serialize(ref bytes, offset, value.P2PEventType, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.Value, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.Command Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __P2PEventType__ = default(global::UniP2P.LLAPI.CommandType);
            var __Value__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __P2PEventType__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.CommandType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.Command();
            ____result.P2PEventType = __P2PEventType__;
            ____result.Value = __Value__;
            return ____result;
        }
    }


    public sealed class PingPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.PingPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.PingPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.PingID);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.PingPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __PingID__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __PingID__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.PingPacket();
            ____result.PingID = __PingID__;
            return ____result;
        }
    }


    public sealed class UdpPacketFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpPacket>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpPacket value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.PeerID, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.UdpPacketL2, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.UdpPacketL2IV, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.UdpPacket Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __PeerID__ = default(string);
            var __UdpPacketL2__ = default(byte[]);
            var __UdpPacketL2IV__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __PeerID__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __UdpPacketL2__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __UdpPacketL2IV__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.UdpPacket();
            ____result.PeerID = __PeerID__;
            ____result.UdpPacketL2 = __UdpPacketL2__;
            ____result.UdpPacketL2IV = __UdpPacketL2IV__;
            return ____result;
        }
    }


    public sealed class UdpPacketL2Formatter : global::MessagePack.Formatters.IMessagePackFormatter<global::UniP2P.LLAPI.UdpPacketL2>
    {

        public int Serialize(ref byte[] bytes, int offset, global::UniP2P.LLAPI.UdpPacketL2 value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.PacketNumber);
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.ACKNumber);
            offset += formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.Command[]>().Serialize(ref bytes, offset, value.Commands, formatterResolver);
            return offset - startOffset;
        }

        public global::UniP2P.LLAPI.UdpPacketL2 Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __PacketNumber__ = default(ulong);
            var __ACKNumber__ = default(ulong);
            var __Commands__ = default(global::UniP2P.LLAPI.Command[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __PacketNumber__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 1:
                        __ACKNumber__ = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                        break;
                    case 2:
                        __Commands__ = formatterResolver.GetFormatterWithVerify<global::UniP2P.LLAPI.Command[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::UniP2P.LLAPI.UdpPacketL2();
            ____result.PacketNumber = __PacketNumber__;
            ____result.ACKNumber = __ACKNumber__;
            ____result.Commands = __Commands__;
            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
