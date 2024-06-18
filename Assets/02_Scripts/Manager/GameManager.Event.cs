using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : SingletonMono<GameManager> 
{

    public void CallEventAddGold(int gold)
    { 
        UserData.Instance.GameData.Gold.Value += gold;
        UserData.Instance.SaveGameData();
    }
    public void CallEventLevelUp(int addLevel)
    {
        UserData.Instance.GameData.Level.Value += addLevel;
        UserData.Instance.SaveGameData();
    }
}
