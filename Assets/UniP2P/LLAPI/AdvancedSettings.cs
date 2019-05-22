using UnityEngine;
using UniP2P.LLAPI;
using UniP2P.Debug;

namespace UniP2P
{
    public class AdvancedSettings : ScriptableObject
    {
        public bool AutoOnLoadInit = true;

        public int Port = 0;

        public int MaxPeer = 5;

        public int TickRate = 60;

        public SocketType SocketType = SocketType.dotnetUDP;

        public SerializerType Serializer = SerializerType.MessagePack;

        public bool isSTUN = true;

        public string STUNURL = "64.233.188.127";

        public int STUNPort = 19302;

        public int LANMatchingBroadcastPort = 9500;

        public ConsoleType[] Consoles = new ConsoleType[] { ConsoleType.System, ConsoleType.Unity, ConsoleType.Extension };
    }
}