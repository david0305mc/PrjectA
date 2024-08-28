using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
    public void GetDamaged(AttackData _attackData);
    public bool IsEnemy();
}

namespace SS
{
    public interface IDamagable
    {
        public void GetDamaged(AttackData2 _attackData);
        public bool IsEnemy();

    }
}
