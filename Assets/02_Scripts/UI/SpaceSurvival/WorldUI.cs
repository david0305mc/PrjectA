using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        SS.UserDataManager.Instance.SavableData.Level.Subscribe(_value =>
        {
            levelText.SetText(_value.ToString());
        }).AddTo(gameObject);

        SS.UserDataManager.Instance.SavableData.Gold.Subscribe(_value =>
        {
            goldText.SetText(_value.ToString());
        }).AddTo(gameObject);
    }
}
