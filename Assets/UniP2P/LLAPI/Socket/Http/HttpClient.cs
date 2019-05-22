using System.Text;
using UniRx.Async;
using UnityEngine.Networking;

namespace UniP2P.LLAPI
{
    public class HttpResult
    {
        public long StatusCode;
        public string Text;

        public HttpResult(long code, string text)
        {
            StatusCode = code;
            Text = text;
        }
    }

    public static class HttpClient
    {
        public async static UniTask<HttpResult> Get(string url)
        {
            var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            return new HttpResult(request.responseCode, request.downloadHandler.text);
        }

        public async static UniTask<HttpResult> Post(string url, string json)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            return new HttpResult(request.responseCode, request.downloadHandler.text);
        }
    }
}
