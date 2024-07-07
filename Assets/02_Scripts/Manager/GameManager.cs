using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game;

namespace SS
{
    public partial class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private MoveObj testMoveObjPrefab;
        [SerializeField] private Vector2Int startPos;
        [SerializeField] private Vector2Int endPos;


        [SerializeField] private Transform objRoot;
        [SerializeField] private MainUI mainUI;
        [SerializeField] private InGameUI ingameUI;
        [SerializeField] private WorldMap worldMap;

        private GridMap gridMap;
        private GameConfig.GameState gameState;
        public List<Boids2D> mapMoveObjList = new List<Boids2D>();
        private Dictionary<long, MoveObj> enemyObjDic = new Dictionary<long, MoveObj>();
        private Dictionary<long, HeroObj> heroObjDic = new Dictionary<long, HeroObj>();

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
            //worldMap.SelectStage(-1);
        }
        public void StartSpaceSurvival(string mapPrefab)
        {
            SetIngameUI();
            worldMap.gameObject.SetActive(false);
            UniTask.Create(async () =>
            {
                currMapOpHandler = Addressables.InstantiateAsync(mapPrefab, Vector3.zero, Quaternion.identity, objRoot);
                await currMapOpHandler;
                gridMap = currMapOpHandler.Result.GetComponent<GridMap>();
            });
        }

        public static long GenerateUID()
        {
            return UserData.Instance.uidSeed++;
        }

        private void SpawnWaveEnemy()
        {
            //MoveObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, gridMap.ObjectField, false);
            //moveObj.InitData(gridMap, startPos.x, startPos.y, endPos.x, endPos.y);
            //moveObj.boidsObjList = mapMoveObjList;
            //mapMoveObjList.Add(moveObj);
            //var enemyData = UserData.Instance.AddEnemyData(1, 100);
            //MoveObj enemyObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, gridMap.ObjectField, false);
            //enemyObjDic.Add(enemyData.battleUID, enemyObj);
            //enemyObj.InitData.InitObject(enemyData.battleUID);

            //Enumerable.Range(0, _waveStageInfo.unitcnt).ToList().ForEach(i =>
            //{
            //    var randNum = Random.Range(0, currStageObj.enemySpawnPos.Count);

            //    UnitBattleData data = UserData.Instance.AddEnemyData(_waveStageInfo.unitid, _waveStageInfo.unitpowerrate);
            //    Vector3 spawnPos = currStageObj.enemySpawnPos[randNum].position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

            //    GameObject unitPrefab = MResourceManager.Instance.GetPrefab(data.refData.prefabname);
            //    MEnemyObj enemyObj = Lean.Pool.LeanPool.Spawn(unitPrefab, spawnPos, Quaternion.identity, objRoot).GetComponent<MEnemyObj>();

            //    enemyObj.InitObject(data.battleUID, true, (_attackData) =>
            //    {
            //        var heroObj = GetHeroObj(_attackData.attackerUID);
            //        if (heroObj == null)
            //        {
            //            // To Do : ??
            //            return;
            //        }
            //        DoEnemyGetDamage(enemyObj, heroObj.transform.position, _attackData.attackerUID, _attackData.damage);
            //    });
            //    enemyDic.Add(data.battleUID, enemyObj);
            //});

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

        public void SpawnTest()
        {
            MoveObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, gridMap.ObjectField, false);
            moveObj.InitData(1, gridMap, startPos, endPos);
            moveObj.boidsObjList = mapMoveObjList;
            mapMoveObjList.Add(moveObj);
        }
        
        public void EnemyAttackHero(long _heroID)
        {
            if (!heroObjDic.ContainsKey(_heroID))
            {
                Debug.LogError($"heroObjDic.ContainsKey {_heroID}");
                return;
            }
            Lean.Pool.LeanPool.Despawn(heroObjDic[_heroID]);
            heroObjDic.Remove(_heroID);
        }

        public void AddHeroObj(HeroObj _obj)
        {
            // To Do : create hero Data
            _obj.UnitUID = GenerateUID();
            heroObjDic.Add(_obj.UnitUID, _obj);
        }
    }

}
