using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class EnemyObj : MoveObj
    {
        public override void InitData(long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
        {
            unitData = SS.UserData.Instance.GetHeroData(_unitUID);
            base.InitData(_unitUID, _mapCreator, _startTile, _endTile);
        }
        protected override void DoAttack()
        {
            if (targetObj != null)
            {
                GameManager.Instance.EnemyAttackHero(targetObj.UnitUID);
            }
            base.DoAttack();
            if (UserData.Instance.GetHeroData(targetObj.UnitUID) == null)
            {
                fsm.ChangeState(UnitStates.Idle);
            }
        }
    }

}
