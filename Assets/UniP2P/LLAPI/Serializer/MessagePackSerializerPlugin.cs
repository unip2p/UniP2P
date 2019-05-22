using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace UniP2P.LLAPI
{
    public class MessagePackSerializerPlugin : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return MessagePackSerializer.Serialize<T>(obj);
        }

        public T Deserialize<T>(byte[] value)
        {
            return MessagePackSerializer.Deserialize<T>(value);
        }

        public byte[] SerializePublicField<T>(T obj)
        {
            return MessagePackSerializer.Serialize<T>(obj, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }

        public T DeserializePublicField<T>(byte[] value)
        {
            return MessagePackSerializer.Deserialize<T>(value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
    }
}
