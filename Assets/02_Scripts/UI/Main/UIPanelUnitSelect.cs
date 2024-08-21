using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UIPanelUnitSelect : MonoBehaviour
{
    //[SerializeField] private UIGridView gridView = default;
    [SerializeField] private UIUnitCell unitCellPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private List<UIBattlePartySlot> battlePartyList = default;

    private List<SS.UnitData> heroDataList;
    private List<UIUnitCell> uiUnitCelLists;

    private void Awake()
    {
        MessageDispather.Receive(EMessage.Update_HeroParty).Subscribe(_ =>
        {
            InitUI();
        }).AddTo(gameObject);
        uiUnitCelLists = new List<UIUnitCell>();
    }
    public void InitUI()
    {
        InitBattlePartyUI();
        InitUserListScroll();
    }

    private void ClearUnitListPool()
    {
        foreach (var item in uiUnitCelLists)
        {
            Lean.Pool.LeanPool.Despawn(item);
        }
        uiUnitCelLists.Clear();
    }

    private void InitUserListScroll()
    {
        if (uiUnitCelLists.Count > 0)
        {
            ClearUnitListPool();
        }

        heroDataList = (from item in SS.UserDataManager.Instance.SavableData.HeroDataDic
                        where item.Value.refData.id != GameDefine.MyBossUnitTID
                        orderby item.Key ascending
                        orderby item.Value.grade descending
                        orderby item.Value.refData.unitrarity descending
                        select item.Value).ToList();

        Enumerable.Range(0, heroDataList.Count).ToList().ForEach(i =>
        {
            UIUnitCell unitCell = Lean.Pool.LeanPool.Spawn(unitCellPrefab, scrollRect.content);
            unitCell.SetData(i, heroDataList[i].uid, (index)=> {
                ShowUnitInfoPopup(heroDataList[index].uid);
            });
            uiUnitCelLists.Add(unitCell);
        });
    }

    public void ShowUnitInfoPopup(long _uid)
    {
        var popup = PopupManager.Instance.Show<UnitInfoPopup>();
        popup.SetData(_uid, () =>
        {
            int partySlotIndex = SS.UserDataManager.Instance.GetPartySlotIndexByUID(_uid);
            if (partySlotIndex == -1)
                // Equip
            {
                if (SS.UserDataManager.Instance.FindEmptySlot() == -1)
                {
                    PopupManager.Instance.ShowSystemOneBtnPopup(LocalizeManager.Instance.GetLocalString("AlertNoEmptySlot"), "OK");
                    return;
                }
                int slotIndex = SS.GameManager.Instance.AddBattleParty(_uid);
            }
            else
            {
                // UnEquip
                SS.GameManager.Instance.RemoveBattleParty(partySlotIndex);
            }
        });
    }
    private void InitBattlePartyUI()
    {
        Enumerable.Range(0, battlePartyList.Count).ToList().ForEach(i =>
        {
            long unitUID = SS.UserDataManager.Instance.GetBattlePartyUIDByIndex(i);
            battlePartyList[i].SetData(i, unitUID, (_slotIndex) =>
            {
                SS.GameManager.Instance.RemoveBattleParty(_slotIndex);
            }); 
        });
    }

    private void ClearBattlePartyPool()
    {
        foreach (var item in battlePartyList)
        {
            item.ClearPool();
        }
    }
    private void OnDisable()
    {
        ClearBattlePartyPool();
        ClearUnitListPool();
    }

}
