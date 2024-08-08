using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSlot : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image iconImage;

    private long unitUID;
    public long UnitUID { get { return unitUID; } }
    public void SetData(long _unitUID)
    {
        unitUID = _unitUID;
        var unitData = SS.UserDataManager.Instance.GetHeroData(_unitUID);
        iconImage.sprite = MResourceManager.Instance.GetSpriteFromAtlas(unitData.refData.thumbnailpath);
        bgImage.sprite = MResourceManager.Instance.GetBuildAtlas($"RatingBG_{unitData.refData.unitrarity}");
    }
}
