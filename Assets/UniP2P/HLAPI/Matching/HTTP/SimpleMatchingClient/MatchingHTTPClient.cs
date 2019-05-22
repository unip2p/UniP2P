using UniRx.Async;
using UniP2P.LLAPI;
using UnityEngine;
using System.Collections.Generic;
using MiniJSON;
using System.Collections;

namespace UniP2P.HLAPI
{
    public static class SimpleMatchingClient
    {
        public const string API_VERSION = "2019-05-01";

        private static string GetURIGamekey(string url)
        {
            if (UniP2PManager.MatchingSettings.MatchingGameKey == string.Empty)
            {
                return url;
            }
            else
            {
                if (url.EndsWith("/"))
                {
                    return url + UniP2PManager.MatchingSettings.MatchingGameKey;
                }
                else
                {
                    return url + "/" + UniP2PManager.MatchingSettings.MatchingGameKey;
                }
            }
        }

        private static string CurrentRoomID;
        private static string CurrentRoomHostToken;
        private static string CurrentToken;
        

        public static async UniTask<bool> GetStatusAsync()
        {
            var result = await HttpClient.Get(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/status");
            if (result.StatusCode == 200)
            {
                return true;
            }
            else
            {
                Debug.Debugger.Error("[SimpleMatchingClient] GetStatusAsync StatusCode:" + result.StatusCode);
                return false;
            }
        }

        public static async UniTask<bool> VersionCheckAsync()
        {
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/version", string.Format("{" + "version:{0}" + "}", API_VERSION));
            if (result.StatusCode == 200)
            {
                return true;
            }
            else
            {
                Debug.Debugger.Error("[SimpleMatchingClient] Don't Match Version.");
                return false;
            }
        }

        public static async UniTask<Room[]> GetRoomsAsync()
        {
            var result = await HttpClient.Get(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms");
            if (result.StatusCode == 200)
            {
                var res = new RoomsResponse().rooms = new List<Room>();
                IList rooms = (IList)Json.Deserialize(result.Text);
                foreach (IDictionary room in rooms)
                {
                    var obj = new Room();
                    obj.roomid = (string)room["roomid"];
                    obj.roomname = (string)room["roomname"];
                    obj.maxmember = (long)room["maxmember"];
                    obj.currentmember = (long)room["currentmember"];
                    obj.metadata = (string)room["metadata"];
                    res.Add(obj);
                }
                return res.ToArray();
            }
            else if(result.StatusCode == 404) 
            {
                return null;
            }
            else
            {
                Debug.Debugger.Warning("[SimpleMatchingClient] GetRoomListAsync StatusCode:" + result.StatusCode);
                return null;
            }
        }

        public static async UniTask<bool> CreateRoomAsync(string roomname,int maxmember, string metadata = "")
        {
            var req = new CreateRoomRequest
            {
                peerid = UniP2PManager.MyPeerID,
                roomname = roomname,
                maxmember = maxmember,
                metadata = metadata
            };
            req.SetHash();
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms/create", JsonUtility.ToJson(req));
            if (result.StatusCode == 200)
            {
                var room = JsonUtility.FromJson<CreateRoomResponse>(result.Text);
                CurrentRoomID = room.roomid;
                CurrentRoomHostToken = room.token;
                return true;
            }
            else
            {
                Debug.Debugger.Error("[SimpleMatchingClient] CreateRoomAsync StatusCode:" + result.StatusCode + ":" +JsonUtility.ToJson(req));
                return false;
            }
        }

        public static async UniTask<bool> JoinRoomAsync()
        {
            return await JoinRoomAsync(CurrentRoomID);
        }

        public static async UniTask<bool> JoinRoomAsync(string roomid, bool isconnect = true, bool beforedisconnect = true)
        {
            var req = new JoinRoomRequest
            {
                peerid = UniP2PManager.MyPeerID,
                roomid = roomid,
                ip = IPEndPointParser.ToString(UniP2PManager.GetEnableIPEndPoint()),
                localport = UniP2PManager.PrivateIPEndPoint.Port
            };
            req.SetHash();
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms/join", JsonUtility.ToJson(req));
            if (result.StatusCode == 200)
            {
                var room = JsonUtility.FromJson<JoinRoomResponse>(result.Text);
                CurrentRoomID = roomid;
                CurrentToken = room.token;
                if (isconnect)
                {
                    if (beforedisconnect)
                    {
                        await UniP2PManager.DisConnectAllPeerAsync();
                    }
                    foreach (var peer in room.peers)
                    {
                        if (peer.id == UniP2PManager.MyPeerID)
                        {
                            continue;
                        }
                        await UniP2PManager.SendEmptyPacketAsync(IPEndPointParser.Parse(peer.ip));
                        await UniTask.Delay(100);
                        await UniP2PManager.ConnectPeerAsync(IPEndPointParser.Parse(peer.ip), peer.id, (int)peer.localport);
                    }
                }

                return true;
            }
            else
            {
                Debug.Debugger.Warning("[SimpleMatchingClient] JoinRoomAsync StatusCode:" + result.StatusCode + ":" + JsonUtility.ToJson(req));
                return false;
            }
        }

        public static async UniTask<bool> JoinRandomRoomAsync(bool isconnect = true, bool beforedisconnect = true)
        {
            var req = new JoinRandomRoomRequest
            {
                peerid = UniP2PManager.MyPeerID,
                ip = IPEndPointParser.ToString(UniP2PManager.GetEnableIPEndPoint())
            };
            req.SetHash();
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms/joinrandom", JsonUtility.ToJson(req));
            if (result.StatusCode == 200)
            {
                var room = JsonUtility.FromJson<JoinRandomRoomResponse>(result.Text);
                CurrentRoomID = room.roomid;
                CurrentToken = room.token;
                if (isconnect)
                {
                    if (beforedisconnect)
                    {
                        await UniP2PManager.DisConnectAllPeerAsync();
                    }

                    List<UniTask<Peer>> tasks = new List<UniTask<Peer>>();
                    foreach (var peer in room.peers)
                    {
                        if (peer.id != UniP2PManager.MyPeerID)
                        {
                            var task = UniP2PManager.ConnectPeerAsync(IPEndPointParser.Parse(peer.ip), peer.id);
                            tasks.Add(task);
                        }
                    }

                    await UniTask.WhenAll(tasks);                    
                }

                return true;
            }
            else
            {
                Debug.Debugger.Warning("[SimpleMatchingClient] JoinRandomRoomAsync StatusCode:" + result.StatusCode + ":" + JsonUtility.ToJson(req));
                return false;
            }
        }

        public static async UniTask<CheckRoomResponse> CheckRoomAsync()
        {
            var req = new CheckRoomRequest
            {
                peerid = UniP2PManager.MyPeerID,
                roomid = CurrentRoomID,
                token = CurrentToken
            };
            req.SetHash();
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms/check", JsonUtility.ToJson(req));
            if (result.StatusCode == 200)
            {
                var room = JsonUtility.FromJson<CheckRoomResponse>(result.Text);
                foreach (var p in room.peers)
                {
                    if (p.id != UniP2PManager.MyPeerID && UniP2PManager.GetConnectedPeer(p.id) == null)
                    {
                        await UniP2PManager.SendEmptyPacketAsync(IPEndPointParser.Parse(p.ip));
                    }
                }
                return room;
            }
            else
            {
                Debug.Debugger.Warning("[SimpleMatchingClient] CheckRoomAsync StatusCode:" + result.StatusCode + ":" + JsonUtility.ToJson(req));
                return null;
            }
        }

        public static async UniTask<bool> CloseRoomAsync()
        {
            var req = new CloseRoomRequest
            {
                peerid = UniP2PManager.MyPeerID,
                roomid = CurrentRoomID,
                token = CurrentRoomHostToken
            };
            req.SetHash();
            var result = await HttpClient.Post(GetURIGamekey(UniP2PManager.MatchingSettings.MatchingServerURI) + "/rooms/close", JsonUtility.ToJson(req));
            if (result.StatusCode == 200)
            {
                return true;
            }
            else
            {
                Debug.Debugger.Warning("[SimpleMatchingClient] CheckRoomAsync StatusCode:" + result.StatusCode + ":" + JsonUtility.ToJson(req));
                return false;
            }
        }

    }
}
