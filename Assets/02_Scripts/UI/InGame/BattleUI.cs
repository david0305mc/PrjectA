using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private List<UIUnitSlot> unitSlotList;

    private void Start()
    {
        CreateUnitSlot();
    }

    private void CreateUnitSlot()
    {
        var heroDataList = UserData.Instance.LocalData.HeroDataDic.ToList();

        for (int i = 0; i < unitSlotList.Count; i++)
        {
            if (heroDataList.Count < i)
            {
                unitSlotList[i].gameObject.SetActive(false);
            }
            else
            {
                unitSlotList[i].gameObject.SetActive(true);
                unitSlotList[i].SetData(heroDataList[i].Value.refData.id);
            }
        }
            
    }

}
