using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniP2P.LLAPI
{
    public class Serializer
    {
        private static ISerializer serializer = new MessagePackSerializerPlugin();

        public static byte[] Serialize<T>(T obj)
        {
            return serializer.Serialize(obj);
        }

        public static T Deserialize<T>(byte[] value)
        {
            return serializer.Deserialize<T>(value);
        }

        public static byte[] SerializePublicField<T>(T obj)
        {
            return serializer.SerializePublicField(obj);
        }

        public static T DeserializePublicField<T>(byte[] value)
        {
            return serializer.DeserializePublicField<T>(value);
        }
    }
}
