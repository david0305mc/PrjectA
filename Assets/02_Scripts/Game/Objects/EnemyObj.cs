using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class EnemyObj : MoveObj
    {
        protected override void DoAttack()
        {
            
            if (targetObj != null)
            {
                GameManager.Instance.EnemyAttackHero(targetObj.UnitUID);
            }
            base.DoAttack();
        }
    }

}
