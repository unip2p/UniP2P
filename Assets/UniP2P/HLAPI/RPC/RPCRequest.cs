using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniP2P.HLAPI
{
    [MessagePackObject]
    public class RPCRequest
    {
        [Key(0)]
        public string MethodName { get; private set; }
        
        [Key(1)]
        public byte[] Parameter { get; private set; }

        public RPCRequest(string name, byte[] parameter)
        {
            MethodName = name;
            Parameter = parameter;
        }
    }
}
