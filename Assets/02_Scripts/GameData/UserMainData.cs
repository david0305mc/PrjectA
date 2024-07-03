using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class UserData : Singleton<UserData>
    {
        public Dictionary<int, UnitBattleData> enemyDataDic;
        public Dictionary<int, UnitBattleData> battleHeroDataDic;
        public UnitBattleData AddEnemyData(int _tid, int _powerRate)
        {
            var data = UnitBattleData.Create(MGameManager.GenerateFlashUID(), -1, _tid, 1, 1, true, _powerRate);
            enemyDataDic.Add(data.battleUID, data);
            return data;
        }

    }

}
