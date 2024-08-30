using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using SS;
public class UIBattlePartySlot : MonoBehaviour
{
    [SerializeField] private GameObject emptySlot;
    [SerializeField] private GameObject lockSlot;
    [SerializeField] private TextMeshProUGUI unlockLvText;
    [SerializeField] private Transform characterViewTr;
    [SerializeField] private Button button;

    public BaseObj heroObj = default;

    private System.Action<int> touchAction;
    private int slotIndex;
    private long unitUID;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            if (unitUID != -1)
            {
                touchAction?.Invoke(slotIndex);
            }
        });
    }

    public void SetData(int _slotIndex, long _unitUID, System.Action<int> _touchAction)
    {
        slotIndex = _slotIndex;
        touchAction = _touchAction;
        if (_unitUID != -1)
        {
            if (_unitUID != unitUID)
            {
                emptySlot.SetActive(false);
                var heroData = SS.UserDataManager.Instance.GetHeroData(_unitUID);
                GameObject unitPrefab = MResourceManager.Instance.GetPrefab(heroData.refData.prefabname);
                heroObj = Lean.Pool.LeanPool.Spawn(unitPrefab, Vector3.zero, Quaternion.identity, characterViewTr).GetComponent<BaseObj>();
                heroObj.transform.SetLocalPosition(Vector3.zero);
            }
            heroObj.SetUIMode(Game.GameConfig.CanvasMainUILayerOrder + slotIndex + 2);
        }
        else
        {
            ClearPool();
        }

        UpdateLockState();
        unitUID = _unitUID;
    }

    private void UpdateLockState()
    {
        if (UserDataManager.Instance.SavableData.UnitSlotCount.Value < slotIndex + 1)
        {
            emptySlot.SetActive(false);
            lockSlot.SetActive(true);
            unlockLvText.SetText($"LEVEL {DataManager.Instance.GetUnlockLevelBySlotIndex(slotIndex + 1)}");
        }
        else
        {
            emptySlot.SetActive(true);
            lockSlot.SetActive(false);
        }
    }

    public void ClearPool()
    {
        if (heroObj != null)
        {
            unitUID = -1;
            heroObj.SetBattleMode();
            Lean.Pool.LeanPool.Despawn(heroObj.gameObject);
            heroObj = null;
        }

    }
}
