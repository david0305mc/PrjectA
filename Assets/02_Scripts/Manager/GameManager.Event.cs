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
            SS.UserData.Instance.SavableData.Gold.Value += _add;
            SS.UserData.Instance.SaveLocalData();
        }
    }

}
