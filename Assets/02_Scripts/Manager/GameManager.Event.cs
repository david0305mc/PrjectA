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
            SS.UserData.Instance.SavableData.Gold.Value += _add;
            SS.UserData.Instance.SaveLocalData();
        }
    }

}
