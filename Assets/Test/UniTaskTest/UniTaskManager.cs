using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

public class UniTaskManager : MonoBehaviour
{
    [SerializeField] private Button uniTaskTest1;
    [SerializeField] private Button uniTaskTest2;
    
    private CancellationTokenSource cancellationTokenSource;
    //async UniTask Start()
    //{
    //    GameUtil.Instance.StartWatch();
    //    GameUtil.Instance.StopAndStart("Begin");

    //    var test = TestFunc(1);
    //    test.ContinueWith(() =>
    //    {
    //        var test2 = TestFunc(2);
    //    }).Forget();

    //    Debug.Log("aaaaa");
    //    //await test;
    //    Debug.Log("bbbb");
    //    //TestFunc(2);
    //    //GameUtil.Instance.StopAndStart("End");
    //}

    private async UniTask TestFunc(int param)
    {
        Debug.Log(param);
        //await UniTask.Delay(1000);
        Debug.Log("Before " + param);
        await UniTask.Yield();
        //await UniTask.Delay(1000);
        Debug.Log("After " + param);
    }

    private void Awake()
    {
        uniTaskTest1.onClick.AddListener(() => {
            UniTask.Create(async () =>
            {
                cancellationTokenSource = new CancellationTokenSource();
                Debug.Log($"Unitask 0");
                await UniTask.Delay(1000, cancellationToken: cancellationTokenSource.Token);
                Debug.Log($"Unitask 1");
            });
        });

        uniTaskTest2.onClick.AddListener(async ()=> {
            await UniTask.Delay(1000);
            cancellationTokenSource.Cancel();
        });
    }
}
