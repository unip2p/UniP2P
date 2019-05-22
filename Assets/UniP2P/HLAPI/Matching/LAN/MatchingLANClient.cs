using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessagePack;
using System.Threading.Tasks;
using UnityEngine;
using UniRx.Async;

namespace UniP2P.HLAPI
{
    public class MatchingLANClient
    {
        public static MatchingLANPacket CurrentInfo;

        public static List<MatchingLANPacket> RoomList = new List<MatchingLANPacket>();

        private static UdpClient JoinUdpClient;

        private static bool isReceive;

        public static async UniTask RequestListRoom()
        {
            CurrentInfo = new MatchingLANPacket
            {
                Event = LANBroadcastEvent.Request,
            };
            var buffer = MessagePackSerializer.Serialize(CurrentInfo);
            if (JoinUdpClient != null)
            {
                JoinUdpClient.Close();
                JoinUdpClient = null;
            }

            JoinUdpClient = new UdpClient(UniP2PManager.AdvancedSettings.LANMatchingBroadcastPort);

            await SendBroadCastAsync(MessagePackSerializer.Serialize(CurrentInfo));
            await ReceiveStartAsync();
        }

        public static void Clost()
        {
            isReceive = false;
        }

        private static async UniTask SendBroadCastAsync(byte[] buffer)
        {
            JoinUdpClient.EnableBroadcast = true;
            JoinUdpClient.Connect(new IPEndPoint(IPAddress.Broadcast, UniP2PManager.AdvancedSettings.LANMatchingBroadcastPort));
            await JoinUdpClient.SendAsync(buffer, buffer.Length);
        }

        private static async UniTask ReceiveStartAsync()
        {
            await ListenLoopAsync();
        }

        private static async UniTask ListenLoopAsync()
        {
            while (isReceive)
            {
                var result = await JoinUdpClient.ReceiveAsync();
                OnReceiveCompleted(result.Buffer);
            }
        }

        private static void OnReceiveCompleted(byte[] buffer)
        {
            var o = MessagePackSerializer.Deserialize<MatchingLANPacket>(buffer);
            switch (o.Event)
            {
                case LANBroadcastEvent.Request:
                    break;
                case LANBroadcastEvent.Response:
                    RoomList.Add(o);
                    break;
            }
        }

        public static async UniTask JoinRoom(int index)
        {
            await JoinRoom(RoomList[index]);
        }

        public static async UniTask JoinRoom(MatchingLANPacket info)
        {
            foreach (var ip in info.PeerIPEndPoints)
            {
                await UniP2PManager.ConnectPeerAsync(IPEndPointParser.Parse(ip));
            }
        }

        public static MatchingLANPacket[] GetRoomList()
        {  
            return RoomList.ToArray();
        }
    }

}
