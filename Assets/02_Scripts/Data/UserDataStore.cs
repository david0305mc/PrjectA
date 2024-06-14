using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UserDataStore : SingletonMono<UserDataStore>
{
    private readonly string key = "moon";

    public AccountData AccountData { get; set; }
    //protected override void OnSingletonAwake()
    //{
    //    base.OnSingletonAwake();
    //    AccountData = new AccountData();
    //}

    //private int gold;
    //public int Gold { 
    //    get { return gold; } 
    //    set { 
    //        gold = value;
    //        AssetUpdateAction?.Invoke();
    //    } 
    //}

    private int seed;
    public int Seed
    {
        get { return seed; }
        set
        {
            seed = value;
            AssetUpdateAction?.Invoke();
        }
    }


    public int GetProducePower()
    {
        return AccountData.Level.Value * 10 * AccountData.Buff.Value;
    }

    public void ProduceGold()
    {
        AccountData.Gold.Value += GetProducePower();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(AccountData));
        //PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(key))
        {
            AccountData = JsonConvert.DeserializeObject<AccountData>(PlayerPrefs.GetString(key));
        }
        else
        {
            AccountData = new AccountData();
            Debug.Log("Load Failed");
        }
        AccountData.SubscribeSave(SaveData);

    }
    public System.Action AssetUpdateAction { get; set; }
}

public class AccountData
{
    public ReactiveProperty<int> Gold = new ReactiveProperty<int>();
    public ReactiveProperty<int> Buff { get; private set; } = new ReactiveProperty<int>();
    public IReactiveProperty<int> Level { get; set; }
    private CompositeDisposable disposables = new CompositeDisposable();

    public void DisposalbleClear()
    {
        disposables.Clear();
    }
    public AccountData()
    {
        Gold.Value = 0;
        Buff.Value = 1;
        Level = new ReactiveProperty<int>
        {
            Value = 1
        };
    }
    public void SubscribeSave(Action saveAction)
    {
        Gold.Subscribe((_gold) =>
        {
            saveAction?.Invoke();
        }).AddTo(disposables);
    }
}
