using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

namespace SS
{
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI expText;

        private void Awake()
        {
            UserDataManager.Instance.SavableData.Exp.Subscribe(_value =>
            {
                expText.SetText(_value.ToString());
            }).AddTo(gameObject);

            UserDataManager.Instance.SavableData.Level.Subscribe(_value =>
            {
                levelText.SetText(_value.ToString());
            }).AddTo(gameObject);

            UserDataManager.Instance.SavableData.Gold.Subscribe(_value =>
            {
                goldText.SetText(_value.ToString());
            }).AddTo(gameObject);
        }
    }

}
