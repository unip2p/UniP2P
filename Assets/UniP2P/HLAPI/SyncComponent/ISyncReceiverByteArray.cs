using UniP2P.LLAPI;

namespace UniP2P.HLAPI
{
    public interface ISyncReceiverByteArray
    {
        void OnReceiveByteArray(byte[] value, Peer peer);

        //void OnReceive<T>(T value, Peer peer);
    }
}
