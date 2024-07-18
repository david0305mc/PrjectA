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
        [SerializeField] private UnitObj testMoveObjPrefab;
        [SerializeField] private Vector2Int startPos;
        [SerializeField] private Vector2Int endPos;


        [SerializeField] private Transform objRoot;
        [SerializeField] private MainUI mainUI;
        [SerializeField] private InGameUI ingameUI;
        [SerializeField] private WorldMap worldMap;

        private GridMap gridMap;
        public GridMap GridMap { get { return gridMap; } }
        private GameConfig.GameState gameState;
        private Dictionary<long, UnitObj> enemyObjDic = new Dictionary<long, UnitObj>();
        private Dictionary<long, UnitObj> heroObjDic = new Dictionary<long, UnitObj>();
        public Dictionary<long, UnitObj> HeroObjDic { get { return heroObjDic; } } 
        public Dictionary<long, UnitObj> EnemyObjDic { get { return enemyObjDic; } }

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

        public void HeroAttackEnemy(long _enemyUID)
        {
            var enemyData = UserData.Instance.GetEnemyData(_enemyUID);
            if (enemyData == null)
            {
                Debug.LogError($"heroObjDic.ContainsKey {_enemyUID}");
                return;
            }
            var enemyObj = enemyObjDic[_enemyUID];
            UserData.Instance.AttackToEnemy(_enemyUID, 1);
            enemyObj.GetAttacked();
            if (enemyData.state == UnitDataStates.Dead)
            {
                if (enemyData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    DestroyBuilding(enemyObj.currTileX, enemyObj.currTileY);
                }
                Lean.Pool.LeanPool.Despawn(enemyObj);
                enemyObjDic.Remove(_enemyUID);
                UserData.Instance.RemoveEnemyData(_enemyUID);
            }
        }

        private void DestroyBuilding(int _gridX, int _gridY)
        {
            var currTile = gridMap.Tiles[_gridX, _gridY];
            currTile.SetTileType(TileType.Normal);
        }

        public void EnemyAttackHero(long _heroUID)
        {
            var heroData = UserData.Instance.GetHeroData(_heroUID);
            if (heroData == null)
            {
                Debug.LogError($"heroObjDic.ContainsKey {_heroUID}");
                return;
            }
            var heroObj = heroObjDic[_heroUID];
            UserData.Instance.AttackToHero(_heroUID, 1);
            heroObj.GetAttacked();
            if (heroData.state == UnitDataStates.Dead)
            {
                if (heroData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    DestroyBuilding(heroObj.currTileX, heroObj.currTileY);
                }
                Lean.Pool.LeanPool.Despawn(heroObj);
                heroObjDic.Remove(_heroUID);
                UserData.Instance.RemoveHeroData(_heroUID);
            }
        }

        public void AddHeroObj(UnitObj _obj, int _tid, int _gridX, int _gridY)
        {
            var heroData = SS.UserData.Instance.AddHeroData(_tid);
            //_obj.InitData(heroData.uid, gridMap, new Vector2Int(_obj.TileX, _obj.TileY), new Vector2Int(0, 0));
            _obj.InitData(true, heroData.uid, gridMap, new Vector2Int(_gridX, _gridY), new Vector2Int(7, 7));
            heroObjDic.Add(_obj.UnitUID, _obj);
        }

        public void AddEnemyObj(int _tid)
        {
            var enemyData = SS.UserData.Instance.AddEnemyData(_tid);
            UnitObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, gridMap.ObjectField, false);
            moveObj.InitData(false, enemyData.uid, gridMap, startPos, endPos);
            enemyObjDic.Add(moveObj.UnitUID, moveObj);
        }
    }

}
