using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public partial class GameManager : SingletonMono<GameManager>
    {

        // �̺�Ʈ ������ ���̺�

        public void TestAddGold(long _add)
        {
            SS.UserDataManager.Instance.SavableData.Gold.Value += _add;
            SS.UserDataManager.Instance.SaveLocalData();
        }
    }

}
