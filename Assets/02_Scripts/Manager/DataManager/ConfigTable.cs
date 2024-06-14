#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
public class ConfigTable : Singleton<ConfigTable>{

	public int WemixServerInfo;
	public int MaxFishCreate;
	public int HeartProductDefaultTime;
	public int CrystalHarvestxMaxCount;
	public int StoneGrowUp_1;
	public int StoneGrowUp_2;
	public int StoneGrowUp_3;
	public int FishTankSetDefaultCount;
	public int HermitCrabDefaultHunger;
	public int HermitCrabMaxHunger;
	public int HeartMaxStack;
	public int CrystalMaxStack;
	public int GemMaxStack;
	public int GoldCrystalMaxStack;
	public int StoneScriptPrintCoolTime;
	public int StoneScriptPrintTouchCount;
	public int StoneScriptDeleteTime;
	public int FishScriptDeleteTime;
	public int UserDefaultHeart;
	public int UserDefaultCrystal;
	public int UserDefaultFreeCash;
	public int HermitCrabFoodMaxCount;
	public int UserHeartBonusDefault;
	public int UserCrystalBonusDefault;
	public int UserHeartBonusAdd;
	public int UserCrystalBonusAdd;
	public int HaveFoodCount_2;
	public int HaveFoodCount_3;
	public int HaveFoodCount_4;
	public int TouchHeartProductDecreaseTermMin;
	public int TouchHeartProductDecreaseTermMax;
	public int TouchHeartProductDecreaseTime;
	public int TouchHeartProductDecreaseTouchMin;
	public int TouchHeartProductDecreaseTouchMax;
	public int CardCompositeMaxCount;
	public int CardCompositeMinCount;
	public int HeartProductRandMin;
	public int HeartProductRandMax;
	public int CrystalProductRandMin;
	public int CrystalProductRandMax;
	public int CardBonusGachaNeedCount;
	public int CardPackMaxStack;
	public int CardPackMaxCount;
	public int CardMaxStack;
	public int CardLockTerm;
	public int WemixFirstSignRewardMailId;
	public int HeartProductDefaultAmount;
	public int CrystalProductDefaultAmount;
	public int SkillShrimpEffectShowTime;
	public int SkillMoonEffectShowTime;
	public int FlowNoticeFirstShowWaitTime;
	public int FlowNoticeShowCount;
	public int FlowNoticeShowTerm;
	public int FlowNoticeShowTime;
	public int FlowNoticeRefreshTime;
	public void LoadConfig(Dictionary<string, Dictionary<string, object>> rowList)
	{
		foreach (var rowItem in rowList)
		{
			var field = typeof(ConfigTable).GetField(rowItem.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			field.SetValue(this, rowItem.Value["value"]);
		}
	}
};
