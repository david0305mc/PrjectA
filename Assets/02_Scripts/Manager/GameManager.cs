using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game;
using System.Linq;
using System;
using System.Threading;

namespace SS
{
    public partial class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private BaseObj testMoveObjPrefab;
        [SerializeField] private Vector2Int startPos;


        [SerializeField] private Transform objRoot;
        [SerializeField] private MainUI mainUI;
        [SerializeField] private InGameUI ingameUI;
        [SerializeField] private WorldMap worldMap;
        [SerializeField] private BattleUI battleUI;

        private GridMap gridMap;
        public GridMap GridMap { get { return gridMap; } }
        public GameDefine.GameState GameState { get; private set; }
        public GameDefine.InGameState InGameState { get; private set; }
        private Dictionary<long, BaseObj> enemyObjDic = new Dictionary<long, BaseObj>();
        private Dictionary<long, BaseObj> heroObjDic = new Dictionary<long, BaseObj>();
        public Dictionary<long, BaseObj> HeroObjDic { get { return heroObjDic; } }
        public Dictionary<long, BaseObj> EnemyObjDic { get { return enemyObjDic; } }

        private CancellationTokenSource cancellationTokenSource;
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
            //worldMap.SelectStage(-1);
        }
        public void StartInGame(string mapPrefab)
        {
            cancellationTokenSource?.Clear();
            cancellationTokenSource = new CancellationTokenSource();

            GameState = GameDefine.GameState.InGame;
            InGameState = GameDefine.InGameState.Ready;
            SetIngameUI();
            worldMap.gameObject.SetActive(false);
            var endTime = GameTime.Get() + 10;
            UniTask.Create(async () =>
            {
                currMapOpHandler = Addressables.InstantiateAsync(mapPrefab, Vector3.zero, Quaternion.identity, objRoot);
                await currMapOpHandler;
                gridMap = currMapOpHandler.Result.GetComponent<GridMap>();
                AddMyBossObj();

                long remainTime = endTime - GameTime.Get();
                battleUI.SetCountDownText(remainTime.ToString());

                // CountDown
                PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(0.1f), true, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationTokenSource.Token, _ =>
                {
                    remainTime = endTime - GameTime.Get();
                    if (remainTime > 0)
                    {
                        battleUI.SetCountDownText(remainTime.ToString());
                    }
                    else
                    {
                        battleUI.SetCountDownText(string.Empty);
                        InGameState = GameDefine.InGameState.Battle;
                        StartSpawnEnemy();
                    }
                }, null);

                for (int i = 0; i < 5; i++)
                {
                    AddBattleHeroObj(GameDefine.TestBuildingTid, UnityEngine.Random.Range(0, gridMap.gridCol), UnityEngine.Random.Range(5, 10));
                }
            });
        }

        private void StartSpawnEnemy()
        {
            cancellationTokenSource?.Clear();
            cancellationTokenSource = new CancellationTokenSource();
            int enemyCount = 100;
            PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(0.2f), true, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationTokenSource.Token, _ =>
            {
                AddBattleEnemyObj(2006);
                if (--enemyCount < 0)
                {
                    cancellationTokenSource?.Clear();
                    cancellationTokenSource = null;
                }
            }, null);
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
            cancellationTokenSource?.Clear();
            cancellationTokenSource = null;
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
            RemoveAllProjectile();
        }
        public BaseObj GetUnitObj(long _uid, bool isEnemy)
        {
            if (isEnemy)
            {
                return  GetEnemyObj(_uid);
            }
            return GetHeroObj(_uid);
        }
        public BaseObj GetHeroObj(long _uid)
        {
            if (heroObjDic.TryGetValue(_uid, out BaseObj heroObj))
            {
                return heroObj;
            }
            return default;
        }
        public BaseObj GetEnemyObj(long _uid)
        {
            if (enemyObjDic.TryGetValue(_uid, out BaseObj enemyObj))
                return enemyObj;
            return default;
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
            if (heroData.state == UnitDataStates.Alive)
            {
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
                    if (MyBossObj != null && MyBossObj.UnitUID == _heroUID)
                    {
                        LoseStage();
                    }
                    RemoveHeroObj(_heroUID);
                }
            }
        }
        public void WinStage()
        {
            TouchBlockManager.Instance.AddLock();
            var stageRewards = DataManager.Instance.GetStageRewards(UserData.Instance.PlayingStage);
            int prevLevel = UserData.Instance.LocalData.Level.Value;
            //AddStageRewards(UserData.Instance.AcquireSoul.Value, stageRewards);
            int currLevel = UserData.Instance.LocalData.Level.Value;
            UserData.Instance.ClearStage(UserData.Instance.PlayingStage);

            var popup = PopupManager.Instance.Show<GameResultPopup>();
            popup.SetData(true, stageRewards, () =>
            {
                RemoveStage();
                BackToWorld();
            }, () =>
            {
            }, () =>
            {
                RemoveStage();
                BackToWorld();
            });

            if (prevLevel < currLevel)
            {
                UniTask.Create(async () =>
                {
                    await UniTask.WaitForSeconds(1f);
                    var popup = PopupManager.Instance.Show<LevelUpPopup>();
                    popup.SetData(currLevel);
                    TouchBlockManager.Instance.RemoveLock();
                });
            }
            else
            {
                TouchBlockManager.Instance.RemoveLock();
            }
        }
        public void AddBattleHeroObj(BaseObj _obj, int _gridX, int _gridY)
        {
            var heroData = SS.UserDataManager.Instance.AddBattleHeroData(_obj.UnitData.tid);
            _obj.InitData(heroData);
            _obj.InitBattleData(gridMap, new Vector2Int(_gridX, _gridY), (_attackData)=> {
                EnemyAttackHero(_attackData.attackerUID, _obj.UnitData.uid);
            });
            heroObjDic.Add(_obj.UnitUID, _obj);
            MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(_obj.UnitUID, new Vector2Int(_gridX, _gridY)));
        }
        public BaseObj GenerateHeroObj(int _tid)
        {
            var unitData = UserDataManager.Instance.GetHeroDataByTID(_tid);
            GameObject unitPrefab = MResourceManager.Instance.GetPrefab(unitData.refData.prefabname);
            var baseObj = Lean.Pool.LeanPool.Spawn(unitPrefab, Instance.GridMap.ObjectField).GetComponent<BaseObj>();
            baseObj.InitData(unitData);
            return baseObj;
        }
        private void AddBattleHeroObj(int _tid, int _gridX, int _gridY)
        {
            var baseObj = GenerateHeroObj(_tid);
            AddBattleHeroObj(baseObj, _gridX, _gridY);
        }
        private void AddMyBossObj()
        {
            var baseObj = GenerateHeroObj(GameDefine.MyBossUnitTID);
            AddBattleHeroObj(baseObj, 5, 3);
            UserDataManager.Instance.MyBossUID = baseObj.UnitUID;
        }
        public void AddBattleEnemyObj(int _tid)
        {
            var enemyData = SS.UserDataManager.Instance.AddEnemyData(_tid);
            BaseObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, new Vector2(-100, -100), Quaternion.identity, gridMap.ObjectField);
            moveObj.InitData(enemyData);
            //gridMap.gridCol
            int randCol = UnityEngine.Random.Range(0, gridMap.gridCol);
            moveObj.InitBattleData(gridMap, new Vector2Int(randCol, startPos.y), (_attackData) => {
                HeroAttackEnemy(_attackData.attackerUID, enemyData.uid);
            });
            enemyObjDic.Add(moveObj.UnitUID, moveObj);
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

        private void RemoveAllProjectile()
        {
            Lean.Pool.LeanPool.DespawnAll();
        }
        public void LauchProjectile(BaseObj attackerObj, long _targetUID)
        {
            var projectileInfo = DataManager.Instance.GetProjectileInfoData(GameDefine.TestMissileID);
            var missile = Lean.Pool.LeanPool.Spawn(MResourceManager.Instance.GetMissile(projectileInfo.prefabname), attackerObj.transform.position, Quaternion.identity, objRoot);
            missile.Shoot(new AttackData2(attackerObj.UnitUID, attackerObj.UnitData.tid, attackerObj.UnitData.refUnitGradeData.attackdmg, attackerObj.UnitData.grade, !attackerObj.UnitData.IsEnemy), GetUnitObj(_targetUID, !attackerObj.UnitData.IsEnemy), projectileInfo.speed);
        }

        public void ShowBoomEffect(AttackData2 _attackData, Vector2 _pos, string name = default)
        {
            var unitGradeInfo = DataManager.Instance.GetUnitGrade(_attackData.attackerTID, 1);

            if (!string.IsNullOrEmpty(unitGradeInfo.boomeffectprefab))
            {
                //var effectPrefab = MResourceManager.Instance.GetPrefab("BoomEffect/Stone_Boom_Effect.prefab");
                var effectPrefab = MResourceManager.Instance.GetPrefab(unitGradeInfo.boomeffectprefab);
                if (effectPrefab == null)
                {
                    Debug.LogError($"effectPrefab == null {unitGradeInfo.boomeffectprefab}");
                    return;
                }
                ExplosionEffect effect = Lean.Pool.LeanPool.Spawn(effectPrefab, _pos, Quaternion.identity, objRoot).GetComponent<ExplosionEffect>();
                effect.SetData(() =>
                {
                    Lean.Pool.LeanPool.Despawn(effect);
                });
            }
        }
    }

}
