using System;
using System.Net;
using UniP2P.Debug;

namespace UniP2P.LLAPI
{
    public class Peer
    {
        public string ID;

        public IPEndPoint IPEndPoint;

        public PeerState State = PeerState.None;

        public Peer()
        {
            UniP2PManager.AddPeer(this);
        }
    }
    
}