using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerSettings 
{
    private static string _common;
    public static string commonUrl => _common ??= string.Format("http://scvcms-cdn.flerogamessvc.com/ab_nft/{0}", true ? "dev" : "live");
    public static string serverName { get; private set; } = "dev";
    public static string gameDataUrl { get; private set; }
    public static string cdnUrl { get; private set; }

    public static void Set(VersionData data)
    {
        cdnUrl = data.cdn_url;
        gameDataUrl = string.Format("{0}/gamedata/dev", cdnUrl);
        var i = cdnUrl.LastIndexOf('/') + 1;
        //serverName = cdnUrl.Substring(i, cdnUrl.Length - i);
        gameDataUrl = string.Format("{0}gamedata/{1}", cdnUrl.Substring(0, i), serverName);
        Debug.LogFormat("[Server/Setting/Url] table {0}", gameDataUrl);
    }
}
