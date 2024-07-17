using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class UserData : Singleton<UserData>
    {
        public long uidSeed;
        private Dictionary<long, SS.UnitData> enemyDataDic = new Dictionary<long, UnitData>();
        private Dictionary<long, SS.UnitData> battleHeroDataDic = new Dictionary<long, UnitData>();

        public Dictionary<long, SS.UnitData> BattleHeroDataDic { get { return battleHeroDataDic; } }
        public UnitData GetHeroData(long _uid)
        {
            return battleHeroDataDic.GetValueOrDefault(_uid);
        }

        public UnitData GetEnemyData(long _uid)
        {
            return enemyDataDic.GetValueOrDefault(_uid);
        }
        public UnitData AddEnemyData(int _tid)
        {
            var data = SS.UnitData.Create(GameManager.GenerateUID(), _tid, 1, 1, true);
            enemyDataDic.Add(data.uid, data);
            return data;
        }

        public UnitData AddHeroData(int _tid)
        {
            var data = UnitData.Create(GameManager.GenerateUID(), _tid, 1, 1, false);
            battleHeroDataDic.Add(data.uid, data);
            return data;
        }
        public void RemoveHeroData(long _uid)
        {
            battleHeroDataDic.Remove(_uid);
        }

        public void RemoveEnemyData(long _uid)
        {
            enemyDataDic.Remove(_uid);
        }

        public void AttackToEnemy(long _enemyUID, int _amt)
        {
            var enemyData = enemyDataDic[_enemyUID];
            enemyData.hp -= _amt;
            if (enemyData.hp <= 0)
            {
                enemyData.state = UnitDataStates.Dead;
            }
        }
        public void AttackToHero(long _heroUID, int _amt)
        {
            var heroData = battleHeroDataDic[_heroUID];
            heroData.hp -= _amt;
            if (heroData.hp <= 0)
            {
                heroData.state = UnitDataStates.Dead;
            }
        }

    }

}
