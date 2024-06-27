using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game;

public partial class GameManager : SingletonMono<GameManager>
{
    [SerializeField] private Transform objRoot;
    [SerializeField] private MainUI mainUI;
    [SerializeField] private InGameUI ingameUI;
    [SerializeField] private WorldMap worldMap;

    private GameConfig.GameState gameState;

    // Spacae Survival
    private AsyncOperationHandle<GameObject> currMapOpHandler;
    private StageObject currMap;
    public void SetWorldUI()
    {
        mainUI.SetActive(true);
        ingameUI.SetActive(false);
    }

    public void SetIngameUI()
    {
        mainUI.SetActive(false);
        mainUI.HideStageInfo();
        ingameUI.SetActive(true);
        worldMap.SelectStage(-1);
    }
    public void StartSpaceSurvival(string mapPrefab)
    {
        SetIngameUI();
        worldMap.gameObject.SetActive(false);
        UniTask.Create(async () =>
        {
            currMapOpHandler = Addressables.InstantiateAsync(mapPrefab, Vector3.zero, Quaternion.identity, objRoot);
            await currMapOpHandler;
            //currStageObj = currStageOpHandler.Result.GetComponent<StageObject>();
        });
    }

    private void DisposeCTS()
    {
        //spawnHeroCts?.Cancel();
        //stageCts?.Cancel();
        //timerCts?.Cancel();
        //followCameraCts?.Cancel();
    }

    public void ExitStage()
    {
        DisposeCTS();
        RemoveStage();
        BackToWorld();
    }
    public void BackToWorld()
    {
        gameState = GameConfig.GameState.MainUI;
        SetWorldUI();
        worldMap.gameObject.SetActive(true);
        worldMap.UpdateWorld();
    }

    public void RemoveStage()
    {
        if (!Addressables.ReleaseInstance(currMapOpHandler))
        {
            Destroy(currMapOpHandler.Result.gameObject);
        }


        //RemoveAllBattleHero();
        //RemoveAllEnemy();
        //RemoveAllProjectile();
    }
}
