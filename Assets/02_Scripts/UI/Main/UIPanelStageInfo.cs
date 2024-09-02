using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIPanelStageInfo : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    private System.Action startBtnAction;

    private void Awake()
    {
        startBtn.onClick.AddListener(() => {

            if (UserData.Instance.GetBattlePartyCount() == 0)
            {
                PopupManager.Instance.ShowSystemOneBtnPopup(LocalizeManager.Instance.GetLocalString("needUnitSetMessage"), "OK");
                return;
            }

            if (UserData.Instance.LocalData.Stamina.Value >= ConfigTable.Instance.StageStartCost)
            {
                //startBtnAction?.Invoke();
                SS.GameManager.Instance.StartInGame("SurvivalMap/GridMap01.prefab");
            }
            else
            {
                PopupManager.Instance.ShowSystemOneBtnPopup("Not Enough Stamina", "OK");
            }
        });
    }

    public void SetData(System.Action _startAction)
    {
        startBtnAction = _startAction;
        
    }

    private void OnLocalize()
    {
    }
}
