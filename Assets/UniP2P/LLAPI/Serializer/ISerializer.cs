using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniP2P.LLAPI
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);
        T Deserialize<T>(byte[] value);

        byte[] SerializePublicField<T>(T obj);
        T DeserializePublicField<T>(byte[] value);
    }
}
