using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private List<int> testUnitTIDList;
    [SerializeField] private List<UIUnitSlot> unitSlotList;
    [SerializeField] private Button spawnEnemyBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Awake()
    {
        countDownText.SetText(string.Empty);
        spawnEnemyBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.AddBattleEnemyObj(2006);
        });

        exitBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.ExitStage();
        });
    }


    private void OnEnable()
    {
        CreateUnitSlot();
    }

    private void CreateUnitSlot()
    {
        for (int i = 0; i < unitSlotList.Count; i++)
        {
            long unitUID = SS.UserDataManager.Instance.GetBattlePartyUIDByIndex(i);
            if (unitUID != -1)
            {
                unitSlotList[i].gameObject.SetActive(true);
                unitSlotList[i].SetData(unitUID);
            }
            else
            {
                unitSlotList[i].gameObject.SetActive(false);
            }

            //if (testUnitTIDList.Count < i)
            //{
            //    unitSlotList[i].gameObject.SetActive(false);
            //}
            //else
            //{
            //    unitSlotList[i].gameObject.SetActive(true);
            //    unitSlotList[i].SetData(testUnitTIDList[i]);
            //}
        }
            
    }

    public void SetCountDownText(string _text)
    {
        countDownText.SetText(_text);
    }


}
