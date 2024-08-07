using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Awake()
    {
        SS.UserDataManager.Instance.SavableData.Gold.Subscribe(_value =>
        {
            goldText.SetText(_value.ToString());
        }).AddTo(gameObject);
    }
}
