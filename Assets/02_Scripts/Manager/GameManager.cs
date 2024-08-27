using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game;
using System.Linq;

namespace SS
{
    public partial class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private BaseObj testMoveObjPrefab;
        [SerializeField] private Vector2Int startPos;
        [SerializeField] private Vector2Int endPos;


        [SerializeField] private Transform objRoot;
        [SerializeField] private MainUI mainUI;
        [SerializeField] private InGameUI ingameUI;
        [SerializeField] private WorldMap worldMap;

        private GridMap gridMap;
        public GridMap GridMap { get { return gridMap; } }
        public GameDefine.GameState GameState { get; private set; }
        private Dictionary<long, BaseObj> enemyObjDic = new Dictionary<long, BaseObj>();
        private Dictionary<long, BaseObj> heroObjDic = new Dictionary<long, BaseObj>();
        public Dictionary<long, BaseObj> HeroObjDic { get { return heroObjDic; } }
        public Dictionary<long, BaseObj> EnemyObjDic { get { return enemyObjDic; } }
        
        public BaseObj MyBossObj { 
            get
            {
                if (heroObjDic.TryGetValue(UserDataManager.Instance.MyBossUID, out BaseObj myboss))
                {
                    return myboss;        
                }
                return default;
            }
        }

        // Spacae Survival
        private AsyncOperationHandle<GameObject> currMapOpHandler;
        private StageObject currMap;

        private void Start()
        {
            InitGame();
        }

        private void InitGame()
        {
            mainUI.InitTabGroup();
            SetWorldUI();
        }

        public void SetWorldUI()
        {
            mainUI.SetActive(true);
            ingameUI.SetActive(false);
            GameState = GameDefine.GameState.MainScene;
        }

        public void SetIngameUI()
        {
            mainUI.SetActive(false);
            mainUI.HideStageInfo();
            ingameUI.SetActive(true);
            GameState = GameDefine.GameState.InGame;
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
                AddMyBossObj();
            });
        }

        public static long GenerateUID()
        {
            return UserDataManager.Instance.SavableData.uidSeed++;
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
            SetWorldUI();
            //worldMap.gameObject.SetActive(true);
            //worldMap.UpdateWorld();
        }

        public void RemoveStage()
        {
            if (!Addressables.ReleaseInstance(currMapOpHandler))
            {
                Destroy(currMapOpHandler.Result.gameObject);
            }

            RemoveAllHeroObj();
            RemoveAllEnemyObj();
            //RemoveAllProjectile();
        }

        public void HeroAttackEnemy(long _heroUID, long _enemyUID)
        {
            var heroData = UserDataManager.Instance.GetBattleHeroData(_heroUID);
            var enemyData = UserDataManager.Instance.GetEnemyData(_enemyUID);
            if (enemyData == null)
            {
                Debug.Log($"heroObjDic.ContainsKey {_enemyUID}");
                return;
            }
            var enemyObj = enemyObjDic[_enemyUID];
            UserDataManager.Instance.AttackToEnemy(_enemyUID, GameDefine.TestEnemyDefeatDamage);
            enemyObj.GetAttacked();
            if (enemyData.state == UnitDataStates.Dead)
            {
                if (enemyData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    DestroyBuilding(enemyObj.currTileX, enemyObj.currTileY);
                }
                RemoveEnemyObj(_enemyUID);
            }
        }

        private void DestroyBuilding(int _gridX, int _gridY)
        {
            var currTile = gridMap.Tiles[_gridX, _gridY];
            currTile.SetTileType(TileType.Normal);
        }

        public BaseObj GetBuildingObj(int _gridX, int _gridY)
        {
            foreach (var item in HeroObjDic)
            {
                if (item.Value.UnitData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    if (_gridX == item.Value.currTileX && _gridY == item.Value.currTileY)
                    {
                        return item.Value;
                    }
                }
            }
            return default;
        }

        public bool HasObjInTile(int _gridX, int _gridY)
        {
            foreach (var item in HeroObjDic)
            {
                if (_gridX == item.Value.currTileX && _gridY == item.Value.currTileY)
                {
                    return true;
                }
            }
            foreach (var item in enemyObjDic)
            {
                if (_gridX == item.Value.currTileX && _gridY == item.Value.currTileY)
                {
                    return true;
                }
            }
            return false;
        }

        public void EnemyAttackHero(long _enemyUID, long _heroUID)
        {
            var enemyData = UserDataManager.Instance.GetEnemyData(_enemyUID);
            var heroData = UserDataManager.Instance.GetBattleHeroData(_heroUID);
            if (heroData == null)
            {
                Debug.Log($"heroObjDic.ContainsKey {_heroUID}");
                return;
            }
            var heroObj = heroObjDic[_heroUID];
            if (heroData.refData.unit_type == UNIT_TYPE.BUILDING)
            {
                UserDataManager.Instance.AttackToHero(_heroUID, GameDefine.TestBuildingDefeatDamage);
            }
            else
            {
                UserDataManager.Instance.AttackToHero(_heroUID, GameDefine.TestHeroDefeatDamage);
            }
            heroObj.GetAttacked();
            if (heroData.state == UnitDataStates.Dead)
            {
                if (heroData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    DestroyBuilding(heroObj.currTileX, heroObj.currTileY);
                }
                RemoveHeroObj(_heroUID);
            }
        }

        public void AddBattleHeroObj(BaseObj _obj, int _tid, int _gridX, int _gridY)
        {
            var heroData = SS.UserDataManager.Instance.AddBattleHeroData(_tid);
            _obj.InitData(true, heroData.uid);
            _obj.InitBattleData(gridMap, new Vector2Int(_gridX, _gridY), new Vector2Int(7, 7));
            heroObjDic.Add(_obj.UnitUID, _obj);
            MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(_obj.UnitUID, new Vector2Int(_gridX, _gridY)));
        }

        private void AddMyBossObj()
        {
            var myBossData = UserDataManager.Instance.GetHeroDataByTID(GameDefine.MyBossUnitTID);
            GameObject unitPrefab = MResourceManager.Instance.GetPrefab(myBossData.refData.prefabname);
            var baseObj = Lean.Pool.LeanPool.Spawn(unitPrefab, SS.GameManager.Instance.GridMap.ObjectField).GetComponent<BaseObj>();
            AddBattleHeroObj(baseObj, GameDefine.MyBossUnitTID, 5, 3);

            UserDataManager.Instance.MyBossUID = baseObj.UnitUID;
        }

        private void RemoveAllHeroObj()
        {
            for (int i = UserDataManager.Instance.BattleHeroDataDic.Count - 1; i >= 0; i--)
            {
                RemoveHeroObj(UserDataManager.Instance.BattleHeroDataDic.ElementAt(i).Key);
            }
        }

        private void RemoveHeroObj(long _heroUID)
        {
            UserDataManager.Instance.RemoveHeroData(_heroUID);
            if (heroObjDic.ContainsKey(_heroUID))
            {
                var heroObj = heroObjDic[_heroUID];
                Lean.Pool.LeanPool.Despawn(heroObj);
                heroObjDic.Remove(_heroUID);
            }
            else
            {
                Debug.LogError($"RemoveHeroObj {_heroUID}");
            }
        }

        private void RemoveEnemyObj(long _enemyUID)
        {
            UserDataManager.Instance.RemoveEnemyData(_enemyUID);
            if (enemyObjDic.ContainsKey(_enemyUID))
            {
                var enemyObj = enemyObjDic[_enemyUID];
                Lean.Pool.LeanPool.Despawn(enemyObj);
                enemyObjDic.Remove(_enemyUID);
            }
            else 
            {
                Debug.LogError($"RemoveEnemyObj {_enemyUID}");
            }
        }

        private void RemoveAllEnemyObj()
        {
            for (int i = UserDataManager.Instance.EnemyDataDic.Count - 1; i >= 0; i--)
            {
                RemoveEnemyObj(UserDataManager.Instance.EnemyDataDic.ElementAt(i).Key);
            }
        }

        public void AddBattleEnemyObj(int _tid)
        {
            var enemyData = SS.UserDataManager.Instance.AddEnemyData(_tid);
            BaseObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, new Vector2(-100, -100), Quaternion.identity, gridMap.ObjectField);
            moveObj.InitData(false, enemyData.uid);
            moveObj.InitBattleData(gridMap, startPos, endPos);
            enemyObjDic.Add(moveObj.UnitUID, moveObj);
        }
    }

}
