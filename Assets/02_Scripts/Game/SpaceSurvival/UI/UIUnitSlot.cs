using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSlot : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image iconImage;

    private int unitTID;
    public int UnitTID { get { return unitTID; } }
    public void SetData(int _unitTID)
    {
        unitTID = _unitTID;
        var unitRef = DataManager.Instance.GetUnitinfoData(_unitTID);
        iconImage.sprite = MResourceManager.Instance.GetSpriteFromAtlas(unitRef.thumbnailpath);
        bgImage.sprite = MResourceManager.Instance.GetBuildAtlas($"RatingBG_{unitRef.unitrarity}");
    }
}
