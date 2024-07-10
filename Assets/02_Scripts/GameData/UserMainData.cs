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

        public UnitData GetHeroData(long _uid)
        {
            return battleHeroDataDic.GetValueOrDefault(_uid);
        }

        public UnitData GetEnemyData(long _uid)
        {
            return enemyDataDic.GetValueOrDefault(_uid);
        }
        public UnitData AddEnemyData(long _tid)
        {
            var data = SS.UnitData.Create(GameManager.GenerateUID(), 1, 1, 1, true);
            enemyDataDic.Add(data.uid, data);
            return data;
        }

        public UnitData AddHeroData(long _tid)
        {
            var data = UnitData.Create(GameManager.GenerateUID(), -1, 1, 1, false);
            battleHeroDataDic.Add(data.uid, data);
            return data;
        }
        public void RemoveHeroData(long _uid)
        {
            battleHeroDataDic.Remove(_uid);
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
