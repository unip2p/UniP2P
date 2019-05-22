using System.Net;
using UniRx;
using UniRx.Async;

namespace UniP2P.LLAPI
{
    public interface ISocket
    {
        void Init();

        IPEndPoint GetSTUNResult();

        int BindPort();

        UniTask<Peer> ConnectPeerAsync(IPEndPoint ip, string peerid = "", int localport = 0);

        UniTask SendPacketAsync(Peer peer,byte[] data, CommandType p2PEvent , SocketQosType qosType);

        UniTask DisConnectAsync(Peer peer);

        void Shutdown();

        Subject<P2PEventArgs> OnReceiveDataChanged();

        UniTask SendEmptyPacketAsync(IPEndPoint ip);

        UniTask<int> SendPingPacketAsync(IPEndPoint ip);
    }

}
