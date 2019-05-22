using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace UniP2P.LLAPI
{
    public class P2PEventArgs : EventArgs
    {
        public Peer peer;

        public CommandType eventType { set; get; }

        public byte[] data { set; get; }

        public P2PEventArgs(Peer p, CommandType t, byte[] d)
        {
            peer = p;
            eventType = t;
            data = d;
        }
    }
}
