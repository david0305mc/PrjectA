using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class UserDataManager : Singleton<UserDataManager>
    {
        public long uidSeed;
        private Dictionary<long, SS.UnitData> enemyDataDic = new Dictionary<long, UnitData>();
        private Dictionary<long, SS.UnitData> battleHeroDataDic = new Dictionary<long, UnitData>();

        public Dictionary<long, SS.UnitData> BattleHeroDataDic { get { return battleHeroDataDic; } }
        public SaveData SavableData { get; private set; }

        public void InitData()
        {

        }

        public void LoadLocalData()
        {
            int newUser = PlayerPrefs.GetInt("IsNewUser", 0);
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

            //AddBattleParty(heroData.uid);
        }

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
