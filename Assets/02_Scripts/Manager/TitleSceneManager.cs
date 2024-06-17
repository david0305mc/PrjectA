using Cysharp.Threading.Tasks;
using Protocols.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField] private Image progressBar;
    private CancellationTokenSource _cancelltaionTokenSource = new CancellationTokenSource();

    private void Start()
    {
        LoadProcess().Forget();
    }

    private async UniTask LoadProcess()
    {
        GameUtil.Instance.StartWatch();
        GameUtil.Instance.StopAndStart("NetworkAPI.GetServerStatus");
        await CheckPermmisionAdTracking();
        var taskServerSatus = NetworkAPI.GetServerStatus(_cancelltaionTokenSource.Token);
        var taskMaintenance = NetworkAPI.GetMainteranceData(_cancelltaionTokenSource.Token);
        
        var serverStatus = await taskServerSatus;
        await CheckVersion(serverStatus);
        var patchTask = CheckPatch(serverStatus);
        await CheckMaintenance(await taskMaintenance);
        await CheckUserTerm();
        await CheckPermissionStorage();
        await SignIn();
        //await patchTask;
        GameUtil.Instance.StopAndStart("CheckLoadData Start");
        //await CheckLoadData();
        await DataManager.Instance.LoadDataAsync();
        await DataManager.Instance.LoadConfigTable();
        GameUtil.Instance.StopAndStart("CheckLoadData End");
        //await GetUserData();
        UserData.Instance.LoadGameData();

        //LoadingSceneManager.LoadScene("3_Game");
        LoadScene();
    }

    public void LoadScene()
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        //UserDataStore.Instance.LoadData();
        StartCoroutine(Load(SceneName.Game));
    }

    private async UniTask CheckPatch(EServerStatus serverStatus)
    {
        //if (serverStatus != EServerStatus.Update_Essential && serverStatus != EServerStatus.Maintenance)
        //{
        //    await DataManager.Instance.Patch(_cancelltaionTokenSource.Token, Progress.Create<ProgressData>(x => {
        //        Debug.Log($"Test Patch Progress  {x.idx}");
        //    }));
        //}
        await DataManager.Instance.Patch(_cancelltaionTokenSource.Token, Progress.Create<ProgressData>(x => {
            //Debug.Log($"Test Patch Progress  {x.idx}");
        }));
    }
    async UniTask CheckVersion(EServerStatus status)
    {
        await UniTask.Yield();

        //if (status == EServerStatus.Update_Essential)
        //{
        //    //[Todo] 마켓 이동
        //    Application.OpenURL("https://www.naver.com");
        //}
    }

    async UniTask GetUserData()
    {
        await UniTask.Yield();
    }

    async UniTask SignIn()
    {
        await UniTask.Yield();
    }
    async UniTask CheckPermissionStorage()
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            return;

        UniTaskCompletionSource<bool> permissionStatus = new UniTaskCompletionSource<bool>();
        PermissionCallbacks permissionCallback = new PermissionCallbacks();
        permissionCallback.PermissionDenied += _ => permissionStatus.TrySetResult(false);
        permissionCallback.PermissionDeniedAndDontAskAgain += _ => permissionStatus.TrySetResult(false);
        permissionCallback.PermissionGranted += _ => permissionStatus.TrySetResult(true);
        Permission.RequestUserPermissions(new string[] { Permission.ExternalStorageRead, Permission.ExternalStorageWrite }, permissionCallback);
        var status = await permissionStatus.Task;

        Debug.LogFormat("[Intro/CheckPermissionStorage] Status : {0}", status);
#endif
    }
    async UniTask CheckUserTerm()
    {
        await UniTask.Yield();
    }
    async UniTask CheckMaintenance(MaintenanceData[] maintenanceData)
    {
        await UniTask.Yield();
    }

    async UniTask CheckPermmisionAdTracking()
    {
        // To Do For IOS
#if UNITY_IOS

#endif
        await UniTask.Yield();
    }
    private IEnumerator Load(string sceneName)
    {
        progressBar.fillAmount = 0f;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= LoadSceneEnd;
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
