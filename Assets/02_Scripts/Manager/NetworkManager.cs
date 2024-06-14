using Cysharp.Threading.Tasks;
using System;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{

    public enum SENDTYPE
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
    public class NetworkManager
    {
        protected static double timeout = 5;
        public static readonly string Domain = "http://localhost:8080/";

        public static async UniTask<T> SendToServer<T>(string url, SENDTYPE sendType, string jsonBody)
        {
            await CheckNetWork();
            string requestUrl = Domain + url;

            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));

            UnityWebRequest request = new UnityWebRequest(requestUrl, sendType.ToString());
            
            if (!string.IsNullOrEmpty(jsonBody))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            SetHeaders(request);

            try
            {
                var res = await request.SendWebRequest().WithCancellation(cts.Token);
                T result = JsonUtility.FromJson<T>(res.downloadHandler.text);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    Debug.Log("Timeout");

                    return await SendToServer<T>(url, sendType, jsonBody);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return default;
            }
            return default;
        }

        private static async UniTask CheckNetWork()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //To Do : Network Connect Error Popup
                Debug.LogError("The network is not connected");
                await UniTask.WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);
                Debug.Log("The network is Connected");
            }
        }
        private static void SetHeaders(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/json");
        }
    }

}
