using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private List<int> testUnitTIDList;
    [SerializeField] private List<UIUnitSlot> unitSlotList;

    private void Start()
    {
        CreateUnitSlot();
    }

    private void CreateUnitSlot()
    {
        for (int i = 0; i < unitSlotList.Count; i++)
        {
            //int unitUID = UserData.Instance.GetBattlePartyUIDByIndex(i);
            //if (unitUID != -1)
            //{
            //    unitSlotList[i].gameObject.SetActive(true);
            //    unitSlotList[i].SetData(unitUID);
            //}
            //else
            //{
            //    unitSlotList[i].gameObject.SetActive(false);
            //}
            
            if (testUnitTIDList.Count < i)
            {
                unitSlotList[i].gameObject.SetActive(false);
            }
            else
            {
                unitSlotList[i].gameObject.SetActive(true);
                unitSlotList[i].SetData(testUnitTIDList[i]);
            }
        }
            
    }

}
