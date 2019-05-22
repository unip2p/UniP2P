using System.Collections;
using System.Collections.Generic;
using UniP2P;
using UniP2P.HLAPI;
using UniRx;
using UniRx.Async;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    public string GamePlaySceneName;

    public GameObject RoomNodePrefab;
    public RectTransform RoomNodesRoot;

    public GameObject CreateRoomPanel;
    public Text StatusText;

    public Button CreateButton;
    public Button CloseCreatePanelButton;
    public InputField CreateName;
    public InputField MaxMember;

    public Button OpenCreateButton;
    public Button ReloadButton;
    public Button JoinRandomRoom;

    public GameObject WaitingRoomPanel;
    public Button CloseRoomButton;
    public Text CurrentMemberText;

    public bool isDebug;
    public Text DebugText;

    private bool isWaiting;
    private bool isPlay;


    async void Start()
    {
        ReloadButton.OnClickAsObservable().Subscribe(async _ => await GetRoomsList());
        CreateButton.OnClickAsObservable().Subscribe(async _ => await CreateRoom(CreateName.text, int.Parse(MaxMember.text)));
        CloseCreatePanelButton.OnClickAsObservable().Subscribe(_ => CreateRoomPanel.SetActive(false));
        OpenCreateButton.OnClickAsObservable().Subscribe(_ => CreateRoomPanel.SetActive(true));
        JoinRandomRoom.OnClickAsObservable().Subscribe(async _ => await SimpleMatchingClient.JoinRandomRoomAsync());
        CloseRoomButton.OnClickAsObservable().Subscribe(async _ => await SimpleMatchingClient.CloseRoomAsync());

        if (await SimpleMatchingClient.GetStatusAsync())
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            if (StatusText != null)
            {
                StatusText.text = "OK";
            }
        }
        else
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            if (StatusText != null)
            {
                StatusText.text = "Error";
            }
        }

        if (isDebug)
        {
            DebugText.text = UniP2PManager.PrivateIPEndPoint + "\n" + UniP2PManager.StunIPEndPoint;
        }
        await AutoReloadRooms();
    }

    private async UniTask CreateRoom(string name, int maxmember)
    {
        if (await SimpleMatchingClient.CreateRoomAsync(name, maxmember))
        {
            await SimpleMatchingClient.JoinRoomAsync();
            isWaiting = true;
            CreateRoomPanel.SetActive(false);
            WaitingRoomPanel.SetActive(true);
            CloseRoomButton.gameObject.SetActive(true);
            await AutoCheckRoom();
        }
    }

    private async UniTask AutoReloadRooms()
    {
        while (!isPlay)
        {
            await GetRoomsList();
            await UniTask.Delay(1000);
        }
    }

    private async UniTask GetRoomsList()
    {
        var result = await SimpleMatchingClient.GetRoomsAsync();

        if (RoomNodesRoot != null)
        {
            foreach (RectTransform child in RoomNodesRoot)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }   
        if (result != null)
        {
            foreach (var room in result)
            {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                    return;
                }
#endif
                var g = Instantiate(RoomNodePrefab, RoomNodesRoot);
                var node = g.GetComponent<RoomNode>();
                node.Set(room.roomid, room.roomname, (int)room.currentmember, (int)room.maxmember , this);
            }
        }
    }

    private async UniTask AutoCheckRoom()
    {
        DataEventManager.IsDataEventQueueing = true;
        while (isWaiting)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            await UniTask.Delay(500);
            await CheckRoom();
        }
    }

    private async UniTask CheckRoom()
    {
        var r = await SimpleMatchingClient.CheckRoomAsync();
        if (r != null)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            CurrentMemberText.text = string.Format("Current Member {0}/5" + "\n"+ "Connected Peer: {1}", r.peers.Length,UniP2PManager.GetPeerConnectedCount());
            if (r.isclose)
            {
                isWaiting = false;
                isPlay = true;
                await SceneManager.LoadSceneAsync(GamePlaySceneName);
                DataEventManager.IsDataEventQueueing = false;
            }
        }
    }

    public async UniTask JoinRoom(string roomid)
    {
        WaitingRoomPanel.SetActive(true);
        CloseRoomButton.gameObject.SetActive(false);
        isWaiting = true;
        var join = SimpleMatchingClient.JoinRoomAsync(roomid);
        var check = AutoCheckRoom();

        await UniTask.WhenAll(join, check);
    }
}
