using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public partial class GameManager : SingletonMono<GameManager>
    {

        // 이벤트 단위로 세이브

        public void TestAddGold(long _add)
        {
            UserDataManager.Instance.SavableData.Gold.Value += _add;
            UserDataManager.Instance.SaveLocalData();
        }
        public void TestLevelUp()
        {
            UserDataManager.Instance.SavableData.Level.Value++;
            UserDataManager.Instance.SaveLocalData();
        }

        public int AddBattleParty(long _heroUID)
        {
            int slotIndex = SS.UserDataManager.Instance.AddBattleParty(_heroUID);
            UserDataManager.Instance.SaveLocalData();
            MessageDispather.Publish(EMessage.Update_HeroParty);
            return slotIndex;
        }

        public void RemoveBattleParty(int _slotIndex)
        {
            UserDataManager.Instance.RemoveBattleParty(_slotIndex);
            MessageDispather.Publish(EMessage.Update_HeroParty);
            UserDataManager.Instance.SaveLocalData();
        }
        public void SummonHero(int _count, int _goldCost, System.Action _hideAction = null)
        {
            var gachaList = DataManager.Instance.GenerateGachaResultList(_count);
            foreach (var item in gachaList)
            {
                var gachaInfo = DataManager.Instance.GetGachaListData(item);
                UserDataManager.Instance.AddHeroData(gachaInfo.unitid, gachaInfo.count);
            }
            var popup = PopupManager.Instance.Show<GachaResultPopup>(_hideAction);
            popup.SetData(gachaList);
            UserDataManager.Instance.SavableData.Gold.Value -= _goldCost;
            UserDataManager.Instance.SaveLocalData();
        }
    }

}
