using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniP2P;
using UniP2P.HLAPI;
using UniP2P.LLAPI;

public class Chat : MonoBehaviour,ISyncReceiverByteArray
{
    public Text ChatLog;
    public InputField ChatInput;
    public SyncGameObject SyncGameObject;


    void Start()
    {
        SyncGameObject = GetComponent<SyncGameObject>();
    }

    public async void SendChat()
    {
        ChatLog.text += "User" + UniP2PManager.GetMyPeerOrder().ToString() + ": " + ChatInput.text + "\n";
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField("User" + UniP2PManager.GetMyPeerOrder().ToString() + ": " + ChatInput.text), typeof(Chat));
    }

    public void OnReceiveByteArray(byte[] value, Peer peer)
    {
        string chat = Serializer.DeserializePublicField<string>(value);
        ChatLog.text += chat + "\n";
    }
}
