using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class EnemyObj : MoveObj
    {
        protected override void Awake()
        {
            isHero = false;
            base.Awake();
        }
        public override void InitData(long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
        {
            unitData = SS.UserData.Instance.GetEnemyData(_unitUID);
            base.InitData(_unitUID, _mapCreator, _startTile, _endTile);
        }
        protected override void DoAttack()
        {
            if (targetObj == null)
            {
                return;
            }

            GameManager.Instance.EnemyAttackHero(targetObj.UnitUID);
            base.DoAttack();
            if (UserData.Instance.GetHeroData(targetObj.UnitUID) == null)
            {
                fsm.ChangeState(UnitStates.Idle);
            }
        }
        protected override MoveObj SearchEnemy()
        {
            base.SearchEnemy();
            MoveObj targetObj = default;
            float distTarget = 0;
            //var detectedObjs = Physics2D.OverlapCircleAll(transform.position, 5, Game.GameConfig.UnitLayerMask);

            foreach (var enemyObj in SS.GameManager.Instance.HeroObjDic.Values)
            {
                if (enemyObj != null)
                {
                    if (SS.UserData.Instance.GetHeroData(enemyObj.UnitUID) == null)
                    {
                        Debug.LogError($"battleHeroDataDic not found {enemyObj.UnitUID}");
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

}
