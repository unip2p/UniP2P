using UnityEngine;
using UnityEngine.UI;
using UniP2P.HLAPI;
using UniRx;

public class RoomNode : MonoBehaviour
{
    public Text NameText;
    public Text MemberText;
    public Text IdText;
    public Button JoinButton;
    public string RoomId;

    private MatchingManager MatchingManager;

    public void Set(string roomid, string name, int member, int maxmember , MatchingManager m)
    {
        RoomId = roomid;
        NameText.text = name;
        MemberText.text = string.Format("{0:D} / {1:D}", member, maxmember);
        IdText.text = RoomId;
        MatchingManager = m;
        JoinSetup();
    }

    public void JoinSetup()
    {
        JoinButton.OnClickAsObservable().Subscribe(async _ => await MatchingManager.JoinRoom(RoomId));
    }


}
