using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public struct ProgressData
{
    public int idx;
    public string fileName;
    public int totalCount;
    public float Value => idx / (float)totalCount;
}


public partial class DataManager 
{
    public async UniTask Patch(CancellationToken cancellationToken, IProgress<ProgressData> progress = null)
    {
        await DBPatcher.Instance.Run(cancellationToken, progress);
    }
}

public class DBPatcher : Singleton<DBPatcher>
{
    class DBFileInfo
    {
        public ulong version;
        public ulong totalFileSize;
        public Dictionary<string, List<ulong>> updateFiles;
    }

    private Dictionary<string, string> _headers;
    private Dictionary<string, string> Headers => _headers ??= new Dictionary<string, string>() { { "Cache-Control", "no-cache, no-store" } };
    private const string Key_DBTableVersion = "DBVersion";
    private const string Key_DBFileVersion = "DBFileVesion_{0}";
    private const string Key_Localization = "Localization";

    public bool IsRunning { get; private set; } = false;

    private void SetDBTableVersion(ulong version)
    {
        PlayerPrefs.SetString(Key_DBTableVersion, version.ToString());
    }

    private ulong GetDBTableVersion()
    {
        ulong version = 0;
        if (!File.Exists(string.Format("{0}/{1}/{2}", PathInfo.DataPath, ServerSettings.serverName, "dbList.json")))
        {
            return version;
        }

        ulong.TryParse(PlayerPrefs.GetString(Key_DBTableVersion), out version);
        return version;
    }

    public async UniTask Run(CancellationToken cancellationToken, IProgress<ProgressData> progress)
    {
        IsRunning = true;
        ProgressData progressData = new ProgressData();
        progressData.idx = 0;

        try
        {
            var dbInfo = await GetDBFileInfo(cancellationToken);
            progressData.totalCount = dbInfo.updateFiles.Count;
            progress?.Report(progressData);

            var itr = dbInfo.updateFiles.GetEnumerator();
            List<UniTask> taskList = new List<UniTask>();
            while (itr.MoveNext())
            {
                progressData.fileName = itr.Current.ToString();

                //var lazyTask = FileDownload(itr.Current, cancellationToken: cancellationToken);
                //taskList.Add(lazyTask);
                var lazyTask = FileDownload(itr.Current, cancellationToken: cancellationToken).ToAsyncLazy();
                taskList.Add(lazyTask.Task);
            }

            GameUtil.Instance.StopAndStart("UniTask.WhenAny");
            while (taskList.Count > 0)
            {
                var task = await UniTask.WhenAny(taskList);
                taskList.RemoveAt(task);
                progressData.idx++;
                progress.Report(progressData);
            }
            //await UniTask.WhenAll(taskList);
            GameUtil.Instance.StopAndStart("UniTask.WhenAny End");
            IsRunning = false;
        }
        catch
        {
            Debug.Log("catch");
        }
    }

    private async UniTask<DBFileInfo> GetDBFileInfo(CancellationToken cancellationToken = default)
    {
        string fileName = "dbList.json";
        string url = $"{ServerSettings.gameDataUrl}/{fileName}";
        ulong serverDBVersion = 0;
        ulong localDBVerion = GetDBTableVersion();

        ulong totalFileSize = 0;
        var updateFiles = new Dictionary<string, List<ulong>>();
        byte[] data = await UnityHttp.Get(url, Headers);

        var serverFileInfo = JsonConvert.DeserializeObject<Dictionary<string, List<ulong>>>(Encoding.UTF8.GetString(data));

        Debug.Log(string.Format("[Patcher/DataTable] LocalDBVersion : {0}", localDBVerion));
        Debug.Log(string.Format("[Patcher/DataTable] ServerDBVersion : {0}", serverDBVersion));

        //if (serverDBVersion > localDBVerion)
        {
            await serverFileInfo.ToUniTaskAsyncEnumerable()
                .Where(x =>
                {
                    if (x.Key == Key_DBTableVersion)
                        return false;
                    return true;
                })
                .ForEachAsync(x =>
                {
                    totalFileSize += x.Value[1];
                    updateFiles.Add(x.Key, x.Value);
                });

            string path = $"{PathInfo.DataPath}/{ServerSettings.serverName}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytesAsync($"{path}/{fileName}", data);
        }

        DBFileInfo result = new DBFileInfo();
        result.version = serverDBVersion;
        result.totalFileSize = totalFileSize;
        result.updateFiles = updateFiles;
        return result;
    }

    private async UniTask FileDownload(KeyValuePair<string, List<ulong>> fileInfo, IProgress<float> progress = null, CancellationToken cancellationToken = default)
    {
        string fileName = fileInfo.Key;
        ulong fileVersion = fileInfo.Value[0];
        ulong fileSize = fileInfo.Value[1];
        string url = string.Format("{0}/{1}", ServerSettings.gameDataUrl, fileName);

        byte[] data = await UnityHttp.Get(url, Headers, progress, cancellationToken);
        if(fileSize > 0 && (ulong)data.LongLength != fileSize)
            throw new UnityWebRequestFileSizeException(fileName);

        await File.WriteAllBytesAsync(string.Format($"{PathInfo.DataPath}/{ServerSettings.serverName}/{fileName}"), data, cancellationToken);
        //return "fileName";
        //await System.Threading.Tasks.Task File.WriteAllBytesAsync($"{PathInfo.DataPath}/{ServerSettings.serverName}/{fileName}", data, cancellationToken);
    }

    /// <summary>업데이트가 불필요한지 검증</summary>    
    private bool VerifyDBFile(KeyValuePair<string, List<ulong>> fileInfo)
    {
        bool ret = false;
        var file = new FileInfo(string.Format("{0}/{1}/{2}", PathInfo.DataPath, ServerSettings.serverName, fileInfo.Key));
        if (!file.Exists)
            return ret;

        //[Todo] 암호화된 경우 파일 크기 비교 예외처리
        if ((ulong)file.Length != fileInfo.Value[1])
            return ret;

        ulong localVersion = 0;
        if (!ulong.TryParse(PlayerPrefs.GetString(string.Format(Key_DBFileVersion, fileInfo.Key)), out localVersion))
            return ret;

        if (localVersion != fileInfo.Value[0])
            return ret;

        return true;
    }

}

/// <summary>파일 크기 예외</summary>
public class UnityWebRequestFileSizeException : Exception
{
    public string FileName { get; }
    private string _msg;

    public UnityWebRequestFileSizeException(string fileName) => FileName = fileName;
    public override string Message => _msg ??= FileName + Environment.NewLine + base.Message;
}
