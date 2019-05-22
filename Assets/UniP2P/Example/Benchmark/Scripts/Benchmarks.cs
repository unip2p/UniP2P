using System;
using System.Collections.Generic;
using UniP2P;
using UniP2P.Debug;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UniP2P.HLAPI;
using UniP2P.LLAPI;

public class Benchmarks : MonoBehaviour , ISyncReceiverByteArray
{
    public Text connectedpeertext;
    public Text logtext;
    public Text pingtext;

    private SyncGameObject SyncGameObject;
     
    private async void Start()
    {        
        SyncGameObject = GetComponent<SyncGameObject>();
        await UniTask.Delay(500);
        connectedpeertext.text = "Connected Peer:" + UniP2PManager.GetPeerConnectedCount();
        LogUpdate();
        await RpcTest();
        LogUpdate();
        await SendData();
        LogUpdate();
        await SendByteArray();
        LogUpdate();
        await UniTask.Delay(500);
        
        foreach (var peer in UniP2PManager.GetAllPeer())
        {
            if (peer.State == PeerState.Connected)
            {
                var ms = await UniP2PManager.Ping(peer);
                pingtext.text = peer.ID + ":"+ ms + "ms" + "\n";
            }
        }
    }

    private void LogUpdate()
    {
        string logs = "";
        foreach (var log in DebbugerMessages.Messages.ToArray())
        {
            logs += log.Message + "\n";
        }
        logtext.text = logs;
    }

    private async UniTask RpcTest()
    {
        Debugger.Log("Send RPC Test.");
        await UniTask.Delay(100);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await UniTask.Delay(200);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await UniTask.Delay(100);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await SyncGameObject.RPC(ReceiveRPC,typeof(Benchmarks),false);
        await UniTask.Delay(100);
        await SyncGameObject.RPC(ReceiveRPC, typeof(Benchmarks), false);
        await SyncGameObject.RPC(ReceiveRPC, typeof(Benchmarks), false);
        await UniTask.Delay(400);
        await SyncGameObject.RPC(ReceiveRPC, typeof(Benchmarks), false);
        await SyncGameObject.RPC(ReceiveRPC, typeof(Benchmarks), false);

        await SyncGameObject.RPC(ReceiveRPC, typeof(Benchmarks), false);

        Debugger.Log("Send RPC Test End.");

        CubeTest();
    }

    private int rpctest = 0;
    
    [P2PRPC]
    public void ReceiveRPC()
    {
        rpctest++;
        if (rpctest % 10 == 0)
        {
            Debugger.Log("Receive RPC Test Clear.");
        }
    }

    private async UniTask SendData()
    {
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "0"}, typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "1"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "2"}, typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "3"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "4"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "5"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "6"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "7"}, typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "8"}, typeof(Benchmarks));
        await SyncGameObject.SendAsync(new BenchmarkData {type = "data" ,value = "9"}, typeof(Benchmarks));
    }

    private async UniTask SendByteArray()
    {
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "0"}), typeof(Benchmarks));
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "1"}), typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "2"}), typeof(Benchmarks));
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "3"}), typeof(Benchmarks));
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "4"}), typeof(Benchmarks));
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "5"}), typeof(Benchmarks));
        await UniTask.Delay(150);
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "6"}), typeof(Benchmarks));
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "7"}), typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "8"}), typeof(Benchmarks));
        await UniTask.Delay(100);
        await SyncGameObject.SendByteArrayAsync(Serializer.SerializePublicField(new BenchmarkData {type = "byte",value = "9"}), typeof(Benchmarks));
    }

    private List<string> datalist = new List<string>();
    private List<string> bytelist= new List<string>();

    public void OnReceiveByteArray(byte[] value, Peer peer)
    {
        var data = Serializer.DeserializePublicField<BenchmarkData>(value);
        if (data.type  == "data")
        {
            datalist.Add(data.value);
        }
        else if(data.type == "byte")
        {
            bytelist.Add(data.value);
        }
    }


    private int count = 30;

    public async void CubeTest()
    {
        for (int i = 0; i < count; i++)
        {
            await SyncGameObject.RPC(ReceiveCube, typeof(Benchmarks), false);
            await UniTask.Delay(500);
        }
    }

    public GameObject Cube;
    int cubei = 0;
    [P2PRPC]
    public void ReceiveCube()
    {
        Instantiate(Cube, Vector3.zero, Quaternion.identity);
        cubei++;

        if (cubei == count -1)
        {
            UniP2P.Debug.Debugger.Log("Cube Complete");
            LogUpdate();
        }
    }

}

public class BenchmarkData
{
    public string type;
    public string value;
}