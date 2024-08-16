using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SS
{
    public class UserDataManager : Singleton<UserDataManager>
    {
        private Dictionary<long, SS.UnitData> enemyDataDic = new Dictionary<long, UnitData>();
        private Dictionary<long, SS.UnitData> battleHeroDataDic = new Dictionary<long, UnitData>();

        public Dictionary<long, UnitData> EnemyDataDic => enemyDataDic;
        public Dictionary<long, SS.UnitData> BattleHeroDataDic => battleHeroDataDic;
        public long MyBossUID { get; set; }
        public bool HasMyBoss { get { return battleHeroDataDic.ContainsKey(MyBossUID); } }
        public SaveData SavableData { get; private set; }

        public void InitData()
        {

        }

        public void LoadLocalData()
        {
            int newUser = PlayerPrefs.GetInt("IsNewUser_02", 0);
            if (newUser == 1)
            {
                try
                {
                    var localData = Utill.LoadFromFile(GameDefine.SaveFilePath);
                    //localData = Utill.EncryptXOR(localData);
                    SavableData = JsonUtility.FromJson<SaveData>(localData);
                }
                catch
                {
                    // NewGame
                    InitNewGameData();
                    return;
                }

                SavableData.UpdateRefData();
                //if (IsOnTutorial())
                //{
                //    // Restart 
                //    InitNewGameData();
                //}
            }
            else
            {
                // NewGame
                InitNewGameData();
            }
        }

        public void SaveLocalData()
        {
            SavableData.LastLoginTime = GameTime.Get();
            var saveData = JsonUtility.ToJson(SavableData);
            //saveData = Utill.EncryptXOR(saveData);
            Utill.SaveFile(GameDefine.SaveFilePath, saveData);
            if (PlayerPrefs.GetInt("IsNewUser", 0) == 0)
            {
                PlayerPrefs.SetInt("IsNewUser", 1);
            }
        }

        private void InitNewGameData()
        {
            SavableData = new SaveData();
            //var heroData = AddHeroData(ConfigTable.Instance.DefaultUnit01, 1);
            var heroData = AddHeroData(11012, 1);
            AddBattleParty(heroData.uid);
            AddHeroData(GameDefine.MyBossUnitTID, 1);
        }
        public int GetPartySlotIndexByUID(long _uid)
        {
            KeyValuePair<int, long> data = SavableData.BattlePartyDic.FirstOrDefault(i => i.Value == _uid);
            if (data.Equals(default(KeyValuePair<int, long>)))
            {
                return -1;
            }
            return data.Key;
        }
        public long GetBattlePartyUIDByIndex(int _index)
        {
            return SavableData.BattlePartyDic[_index];
        }
        public UnitData GetHeroDataByTID(int _tid)
        {
            return SavableData.HeroDataDic.Values.FirstOrDefault(item => item.tid == _tid);
        }
        public UnitData GetBattleHeroData(long _uid)
        {
            return battleHeroDataDic.GetValueOrDefault(_uid);
        }
        public UnitData GetHeroData(long _uid)
        {
            return SavableData.HeroDataDic.GetValueOrDefault(_uid);
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

        public UnitData AddBattleHeroData(int _tid)
        {
            var data = UnitData.Create(GameManager.GenerateUID(), _tid, 1, 1, false);
            battleHeroDataDic.Add(data.uid, data);
            return data;
        }

        public UnitData AddHeroData(int _tid, int _count)
        {
            var heroData = SavableData.HeroDataDic.FirstOrDefault(item => item.Value.tid == _tid);
            if (heroData.Equals(default(KeyValuePair<long, UnitData>)))
            {
                var data = UnitData.Create(SS.GameManager.GenerateUID(), _tid, 1, _count, false);
                SavableData.HeroDataDic.Add(data.uid, data);
                return data;
            }
            else
            {
                heroData.Value.count += _count;
                return heroData.Value;
            }
        }
        public int GetUnitSlotCount()
        {
            return DataManager.Instance.GetLevelData(3).unlockslot;
        }
        public int FindEmptySlot()
        {
            for (int i = 0; i < GetUnitSlotCount(); i++)
            {
                if (SavableData.BattlePartyDic[i] == -1)
                {
                    return i;
                }
            }
            return -1;
        }
        public int AddBattleParty(long _heroUID)
        {
            int emptySlotIndex = FindEmptySlot();
            SavableData.BattlePartyDic[emptySlotIndex] = _heroUID;
            CalcBattlePower();
            return emptySlotIndex;
        }

        public void RemoveBattleParty(int _slotIndex)
        {
            SavableData.BattlePartyDic[_slotIndex] = -1;
            CalcBattlePower();
        }
        private void CalcBattlePower()
        {
            //BattlePower = 0;
            //foreach (var item in LocalData.BattlePartyDic)
            //{
            //    if (item.Value != -1)
            //    {
            //        var heroData = GetHeroData(item.Value);
            //        BattlePower += heroData.refUnitGradeData.combatpower;
            //    }
            //}
        }

        public void RemoveHero(int _heroUID)
        {
            SavableData.HeroDataDic.Remove(_heroUID);
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
