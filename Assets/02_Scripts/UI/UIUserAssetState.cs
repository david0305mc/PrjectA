using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIUserAssetState : MonoBehaviour
{
    [SerializeField] private Text goldText;
    [SerializeField] private Text seedText;

    private void Start()
    {
        UserDataStore.Instance.AccountData.Gold.Subscribe((_coin) =>
        {
            goldText.text = UserDataStore.Instance.AccountData.Gold.Value.ToString();
        }).AddTo(this);
    }
    
    public void UpdateUI()
    {
        goldText.text = UserDataStore.Instance.AccountData.Gold.Value.ToString();
        seedText.text = UserDataStore.Instance.Seed.ToString();
    }
}
