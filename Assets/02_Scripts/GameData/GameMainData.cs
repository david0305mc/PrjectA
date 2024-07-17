using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{

    [System.Serializable]
    public class UnitData
    {
        public long uid;
        public int tid;
        public bool IsEnemy;
        public int maxHp;
        public int hp;
        public int grade;
        public int count;

        public DataManager.Unitinfo refData;
        public UnitDataStates state;

        public static UnitData Create(long _uid, int _tid, int _grade, int _count, bool _isEnemy)
        {
            UnitData data = new UnitData()
            {
                uid = _uid,
                tid = _tid,
                grade = _grade,
                IsEnemy = _isEnemy,
                count = _count,
                state = UnitDataStates.Alive,
            };
            data.UpdateRefData();
            data.maxHp = 3;
            data.hp = data.maxHp;

            return data;
        }
        public void UpdateRefData()
        {
            refData = DataManager.Instance.GetUnitinfoData(tid);
            //refUnitGradeData = DataManager.Instance.GetUnitGrade(tid, grade);
        }
    }
}

