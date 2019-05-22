using MessagePack;
using System.Net;
using System.Net.Sockets;
using UniRx.Async;
using UnityEngine;

namespace UniP2P.HLAPI
{
    public static class MatchingUDPLANHost
    {
        public static MatchingLANPacket CurrentInfo;

        private static UdpClient CreateUdpClient;

        private static bool isReceive;

        public static async UniTask CreateRoom(string roomname, int maxmember)
        {
            CurrentInfo = new MatchingLANPacket
            {
                Event = LANBroadcastEvent.Response,
                GameKey = Application.productName,
                RoomName = roomname,
                Peerid = UniP2PManager.MyPeerID,
                MaxMember = maxmember,                
            };
            await Init();
        }

        public static void CloseRoom()
        {
            isReceive = false;
        }

        private static async UniTask Init()
        {
            CreateUdpClient = new UdpClient(UniP2PManager.AdvancedSettings.LANMatchingBroadcastPort);
            isReceive = true;
            await ReceiveStartAsync();
        }

        private static async UniTask ListenLoopAsync()
        {
            while (isReceive)
            {
                await ReceiveStartAsync();
            }
        }

        private static async UniTask ReceiveStartAsync()
        {
            var result = await CreateUdpClient.ReceiveAsync();
            OnReceiveCompleted(result.Buffer, result.RemoteEndPoint);
        }

        private static async void OnReceiveCompleted(byte[] buffer, IPEndPoint remote)
        {
            var o = MessagePackSerializer.Deserialize<MatchingLANPacket>(buffer);
            switch (o.Event)
            {
                case LANBroadcastEvent.Request:
                    if (CurrentInfo != null)
                    {
                        CreateUdpClient.Connect(remote);
                        var buf = MessagePackSerializer.Serialize(CurrentInfo);
                        await CreateUdpClient.SendAsync(buf, buf.Length);
                    }
                    break;
            }
        }

    }

}