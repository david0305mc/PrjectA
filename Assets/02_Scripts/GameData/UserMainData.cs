using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class UserData : Singleton<UserData>
    {
        public long uidSeed;
        public Dictionary<long, SS.UnitData> enemyDataDic = new Dictionary<long, UnitData>();
        public Dictionary<long, SS.UnitData> battleHeroDataDic = new Dictionary<long, UnitData>();

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

    }

}
