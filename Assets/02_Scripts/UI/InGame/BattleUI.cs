using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Image unitIconImage;
    [SerializeField] private Image unitBGImage;
    private UnitData unitData;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        unitData = UserData.Instance.LocalData.HeroDataDic.Values.First();

        unitIconImage.sprite = MResourceManager.Instance.GetSpriteFromAtlas(unitData.refData.thumbnailpath);
        unitBGImage.sprite = MResourceManager.Instance.GetBuildAtlas($"RatingBG_{(int)unitData.refData.unitrarity}");
    }


}
