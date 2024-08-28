using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SS
{

    [System.Serializable]
    public class SaveData
    {
        public int uidSeed;
        public long LastLoginTime;
        public ReactiveProperty<int> Level;
        public ReactiveProperty<long> Gold;
        public SerializableDictionary<long, UnitData> HeroDataDic;
        public SerializableDictionary<int, long> BattlePartyDic;

        public SaveData()
        {
            uidSeed = 10000;
            Gold = new ReactiveProperty<long>(ConfigTable.Instance.GoldDefault);
            Level = new ReactiveProperty<int>(1);
            HeroDataDic = new SerializableDictionary<long, UnitData>();
            BattlePartyDic = new SerializableDictionary<int, long>();
            Enumerable.Range(0, Game.GameConfig.MaxBattlePartyCount).ToList().ForEach(i =>
            {
                BattlePartyDic[i] = -1;
            });
        }

        public void UpdateRefData()
        {
            foreach (var item in HeroDataDic)
                item.Value.UpdateRefData();

        }
    }

    [System.Serializable]
    public class UnitData
    {
        public long uid;
        public int tid;
        public bool IsEnemy;
        public int hp;
        public int maxHp;
        public int grade;
        public int count;

        public DataManager.Unitinfo refData;
        public DataManager.UnitGradeInfo refUnitGradeData;
        public UnitDataStates state;
        public bool IsMaxGrade => grade >= refData.maxgrade;

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
            data.maxHp = data.refUnitGradeData.hp;
            data.hp = data.refUnitGradeData.hp;

            return data;
        }
        public void UpdateRefData()
        {
            refData = DataManager.Instance.GetUnitinfoData(tid);
            refUnitGradeData = DataManager.Instance.GetUnitGrade(tid, grade);
        }
    }


    public class AttackData2
    {
        public long attackerUID;
        public int attackerTID;
        public int damage;
        public int grade;
        public bool attackToEnemy;

        public AttackData2(long _uid, int _tid, int _damage, int _grade, bool _attackToEnemy)
        {
            attackerUID = _uid;
            attackerTID = _tid;
            damage = _damage;
            grade = _grade;
            attackToEnemy = _attackToEnemy;
        }
    }
}

