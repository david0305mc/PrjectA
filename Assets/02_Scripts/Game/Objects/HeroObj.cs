using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeroObj : MoveObj
{
    public int TileX;
    public int TileY;

    public static int spawnCount = 0;

    protected override void Awake()
    {
        isHero = true;
        base.Awake();
    }

    public override void InitData(long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
    {
        unitData = SS.UserData.Instance.GetHeroData(_unitUID);
        base.InitData(_unitUID, _mapCreator, _startTile, _endTile);
    }
    
    public void DragToTarget(Vector2 _target, int _tileX, int _tileY)
    {
        Debug.Log($"STest drag Count {++spawnCount}");
        TileX = _tileX;
        TileY = _tileY;

        //fsm = StateMachine<UnitStates>.Initialize(this, UnitStates.Drag);

        UniTask.Create(async () =>
        {
            while (Vector2.Distance(transform.position, _target) > 0.1f)
            {
                await UniTask.Yield();
                var newPos = Vector2.MoveTowards(transform.position, _target, 3f * Time.deltaTime);
                transform.position = newPos;
            }
            Debug.Log($"STest spawn Count {spawnCount}");
            SS.GameManager.Instance.AddHeroObj(this);
        });
        transform.position = _target;
    }
    protected override MoveObj SearchEnemy()
    {
        MoveObj targetObj = default;
        float distTarget = 0;
        //var detectedObjs = Physics2D.OverlapCircleAll(transform.position, 5, Game.GameConfig.UnitLayerMask);

        foreach (var enemyObj in SS.GameManager.Instance.EnemyObjDic.Values)
        {
            if (enemyObj != null)
            {
                if (SS.UserData.Instance.GetEnemyData(enemyObj.UnitUID) == null)
                {
                    Debug.LogError($"battleEnemyDataDic not found {enemyObj.UnitUID}");
                    continue;
                }
                float dist = Vector2.Distance(enemyObj.transform.position, transform.position);
                if (targetObj == default)
                {
                    targetObj = enemyObj;
                    distTarget = dist;
                }
                else
                {
                    if (distTarget > dist)
                    {
                        // change Target
                        targetObj = enemyObj;
                        distTarget = dist;
                    }
                }
            }
        }
        return targetObj;
    }

}
