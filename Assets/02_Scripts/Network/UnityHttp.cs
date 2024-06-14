using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class ResposeData
{
    private readonly byte[] bytes;
    public Dictionary<string, string> ReponseHeaders { get; }
    public T GetResult<T>() => JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
    public ResposeData(byte[] bytes, Dictionary<string, string> responseHeaders)
    {
        this.bytes = bytes;
        this.ReponseHeaders = responseHeaders;
    }
}
public static class UnityHttp 
{

    private static Dictionary<string, string> jsonHeaders = new Dictionary<string, string>() { { "Content-Type", "application/json" } };

    public static async UniTask<T> Get<T>(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null, CancellationToken cancellationToken = default)
    {
        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(await Get(url, headers, progress, cancellationToken)));
    }

    public static async UniTask<byte[]> Get(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null, CancellationToken cancellationToken = default)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    req.SetRequestHeader(header.Key, header.Value);
                }
            }

            try
            {
                await req.SendWebRequest().ToUniTask(progress, cancellationToken: cancellationToken);
                //Debug.Log($"UnityHttp Get {req.downloadHandler.text}");
                return req.downloadHandler.data;
            }
            catch(UnityWebRequestException e)
            {
                Debug.LogErrorFormat("[UnityHttp] {0}\n{1}", url, e);
                return null;
            }
        }
    }

    public static async UniTask<ResposeData> GetData(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null, CancellationToken cancellationToken = default)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {   
            try
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        req.SetRequestHeader(header.Key, header.Value);
                    }
                }
                await req.SendWebRequest().ToUniTask(progress, cancellationToken: cancellationToken);
                return new ResposeData(req.downloadHandler.data, req.GetResponseHeaders());
            }
            catch
            {
                return null;
            }
        }
    }
}
