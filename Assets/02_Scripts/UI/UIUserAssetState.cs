using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIUserAssetState : MonoBehaviour
{
    [SerializeField] private Text goldText;
    [SerializeField] private Text levelText;

    private void Awake()
    {
        UserData.Instance.GameData.Gold.Subscribe(_gold =>
        {
            goldText.text = _gold.ToString();
        }).AddTo(gameObject);
        UserData.Instance.GameData.Level.Subscribe(_level =>
        {
            levelText.text = _level.ToString();
        }).AddTo(gameObject);
    }
}
