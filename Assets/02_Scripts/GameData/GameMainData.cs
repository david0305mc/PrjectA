using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{

    [System.Serializable]
    public class UnitData
    {
        public int uid;
        public int tid;
        public bool IsEnemy;
        public int hp;
        public int grade;
        public int count;
        public DataManager.Unitinfo refData;
        public DataManager.UnitGradeInfo refUnitGradeData;
        public bool IsMaxGrade => grade >= refData.maxgrade;

        public static UnitData Create(int _uid, int _tid, int _grade, int _count, bool _isEnemy)
        {
            UnitData data = new UnitData()
            {
                uid = _uid,
                tid = _tid,
                grade = _grade,
                IsEnemy = _isEnemy,
                count = _count,
            };
            data.UpdateRefData();
            data.hp = data.refUnitGradeData.hp;

            return data;
        }
        public void UpdateRefData()
        {
            //refData = DataManager.Instance.GetUnitinfoData(tid);
            //refUnitGradeData = DataManager.Instance.GetUnitGrade(tid, grade);
        }
    }
    public class UnitBattleData : UnitData
    {
        public int killCount;
        public bool isDead;
        public int battleUID;
        public int attackDamage;
        public static UnitBattleData Create(int _battleUID, int _uid, int _tid, int _grade, int _count, bool _isEnemy, int powerRate)
        {
            UnitBattleData data = new UnitBattleData()
            {
                battleUID = _battleUID,
                uid = _uid,
                tid = _tid,
                grade = _grade,
                IsEnemy = _isEnemy,
                count = _count,
                killCount = 0,
                isDead = false
            };
            data.UpdateRefData();
            data.hp = (int)(data.refUnitGradeData.hp * powerRate * 0.01f);
            data.attackDamage = (int)(data.refUnitGradeData.attackdmg * powerRate * 0.01f);
            return data;
        }
    }
}

