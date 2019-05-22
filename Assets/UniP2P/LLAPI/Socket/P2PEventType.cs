using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniP2P.LLAPI
{
    public enum CommandType
    {
        Nothing = 0,

        ConnectRequest = 1,

        RequestAccept = 2,
        
        RequestKey = 3,
        
        AcceptKey = 4,
        
        HeartBeat = 5,

        Disconnect = 6,
        
        DataEvent = 7,
        
        PingRequest = 8,
        
        PingResponse = 9
    }
}
