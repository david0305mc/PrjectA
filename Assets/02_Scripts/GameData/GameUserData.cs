using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameUserData
{
    public int uidSeed;
    public ReactiveProperty<int> Level;
    public ReactiveProperty<long> Gold;

    public GameUserData()
    {
        uidSeed = 1000;
        Level = new ReactiveProperty<int>(1);
        Gold = new ReactiveProperty<long>(ConfigTable.Instance.GoldCrystalMaxStack);
    }

    public void UpdateRefData()
    {

    }
}
