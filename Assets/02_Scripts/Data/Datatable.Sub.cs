using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Reflection;

public partial class Datatable
{
    //// 빌리져 그룹 리스트
    //private Dictionary<int, List<int>> mVillagerGroupDic;

    public void MakeClientDT()
    {
        //GenerateVillagerGroupList();
    }

//    #region Resource

//    public string GetResourcePath(int _resID)
//    {
//        var resourceData = GetResourcesData(_resID);
//        return resourceData.path;
//    }

//    #endregion


//    #region Rank
//    public List<Rank_reward> GetRankRewardByContent(CONTENT_GROUP _content)
//    {
//        var item = (from data in dtRank_reward
//                    where data.Value.contents_group == _content
//                    orderby data.Key ascending
//                    select data.Value).ToList();
//        return item;
//    }

//    #endregion

//    #region Structure
//    public _Datatable.Structure_level GetStructureLevelDataByLevel(int _structureIndex, int _level)
//    {
//        return dtStructure_level.Values.Where(item => item.structure_group == _structureIndex && item.level == _level).FirstOrDefault();
//    }
//    public int GetMaxStructureLevel(int _structureIndex)
//    {
//        return dtStructure_level.Values.Where(item => item.structure_group == _structureIndex).Max(ItemData => ItemData.level);
//    }

//    #endregion

//    #region Myhouse
//    public List<_Datatable.Myhouse_voice> GetMyhouseVoice(int _villager)
//    {
//        return (from item in dtMyhouse_voice
//                where item.Value.villager_index == _villager
//                orderby item.Key ascending
//                select item.Value).ToList();
//    }

//    #endregion

//    #region Villager

//    private void GenerateSubVillagerTableOld()
//    {
//        oldQuestGroupDic = new Dictionary<QST_TYPE, Dictionary<int, Dictionary<int, Quest>>>();
//        var enumertor = dtQuest.GetEnumerator();
//        while (enumertor.MoveNext())
//        {
//            var questType = enumertor.Current.Value.qst_type;
//            if (!oldQuestGroupDic.ContainsKey(questType))
//                oldQuestGroupDic[questType] = new Dictionary<int, Dictionary<int, Quest>>();

//            var groupID = enumertor.Current.Value.qst_group;
//            if (!oldQuestGroupDic[questType].ContainsKey(groupID))
//                oldQuestGroupDic[questType][groupID] = new Dictionary<int, Quest>();

//            var order = enumertor.Current.Value.qst_order;
//            oldQuestGroupDic[questType][groupID][order] = enumertor.Current.Value;
//        }
//    }
//    private void GenerateSubVillagerTable()
//    {
//        questGroupDic = new Dictionary<int, Dictionary<int, Quest>>();
//        var enumertor = dtQuest.GetEnumerator();
//        while (enumertor.MoveNext())
//        {
//            var groupID = enumertor.Current.Value.qst_group;
//            if (!questGroupDic.ContainsKey(groupID))
//                questGroupDic[groupID] = new Dictionary<int, Quest>();

//            var order = enumertor.Current.Value.qst_order;
//            questGroupDic[groupID][order] = enumertor.Current.Value;
//        }
//    }
//    private void GenerateVillagerGroupList()
//    {
//        var tempList = from fair in dtVillager
//                       orderby fair.Value.order descending
//                       select fair;
//        mVillagerGroupDic = new Dictionary<int, List<int>>();

//        foreach (var item in tempList)
//        {
//            if (!mVillagerGroupDic.ContainsKey(item.Value.villager_group))
//                mVillagerGroupDic[item.Value.villager_group] = new List<int>();
//            mVillagerGroupDic[item.Value.villager_group].Add(item.Value.index);
//        }
//    }

//    public List<int> GetVillagerGroupList()
//    {
//        return mVillagerGroupDic.Keys.ToList();
//    }

//    public List<int> GetVillagerList(int _groupID)
//    {
//        return mVillagerGroupDic[_groupID];
//    }
//    public string GetVillagerFavorMent(int _favorIndex, int _favorLevel = 1)
//    {
//        if (_favorIndex == 0)
//            return string.Empty;

//        var data = dtVillager_favor.Where(item => (item.Value.favor_index == _favorIndex && item.Value.favor_level <= _favorLevel)).ToList();
//        int randNum = Random.Range(0, data.Count);
//        return data[randNum].Value.favor_title;
//    }

//    public List<Villager_favor> GetVillagerFavorInfoLists(int _favorIndex)
//    {
//        return (from item in dtVillager_favor
//                where item.Value.favor_index == _favorIndex
//                orderby item.Key ascending
//                select item.Value).ToList();
//    }

//    public _Datatable.Villager_favor GetVillagerFavorInfo(int _favorIndex, int _favorLevel)
//    {
//        var data = dtVillager_favor.Where(item => (item.Value.favor_index == _favorIndex && item.Value.favor_level == _favorLevel)).ToList();
//        if (data.Count > 0)
//            return data[0].Value;
//        else
//            return null;
//    }

//    public int GetMaxFavorLevel(int _favorIndex)
//    {
//        return dtVillager_favor.Where(item => item.Value.favor_index == _favorIndex).Max(item => item.Value.favor_level);
//    }

//    public int GetMaxVillagerLevel(GRADE_TYPE _gradeType)
//    {
//        return dtVillager_level.Where(item => item.Value.villager_grade == _gradeType).Max(Item => Item.Value.villager_level);
//    }

//    public int GetMaxEquipUpgrade(GRADE_TYPE _gradeType)
//    {
//        return dtEquip_upgrade.Where(item => item.Value.equip_grade == _gradeType).Max(Item => Item.Value.equip_upgrade_level);
//    }
//    public _Datatable.Equip_upgrade GetEquipUpgrade(GRADE_TYPE _gradeType, int _upgradeLevel)
//    {
//        var data = dtEquip_upgrade.Where(item => (item.Value.equip_grade == _gradeType && item.Value.equip_upgrade_level == _upgradeLevel)).ToList();
//        if (data.Count > 0)
//            return data[0].Value;
//        else
//            return null;
//    }

//    public _Datatable.Villager_level GetVillagerLevelInfo(GRADE_TYPE _villagerGrade, int _villagerLevel)
//    {
//        return dtVillager_level.Where(item => (item.Value.villager_grade == _villagerGrade && item.Value.villager_level == _villagerLevel)).First().Value;
//    }

//    #endregion

//    #region Item

//    #endregion

//    #region Adjust

//    public _Datatable.adjust_data GetAdjustInfo(string type, int typeIndex)
//    {
//        var data = _Datatable.Instance.dtadjust_data.Where(item => item.Value.event_type == type && item.Value.type_index == typeIndex).ToList();
//        if (data.Count > 0)
//            return data[0].Value;
//        else
//            return null;
//    }
//    #endregion


//    #region Event
//    public Bingo_mission[] GetBingo_missionData_byGroupId(int _group)
//    {
//        return _Datatable.Instance.GetBingo_missionData().Values.Where(item => item.group.Equals(_group)).ToArray();
//    }
//    public Bingo GetBingoData_byOrderId(int _group, int _order)
//    {
//        return _Datatable.Instance.GetBingoData().Values.Where(item => item.group.Equals(_group) && item.order.Equals(_order)).FirstOrDefault();
//    }
//    #endregion

//    #region Text

//    public Dictionary<string, Text_ui> dtBuiltInText_ui = new Dictionary<string, Text_ui>();
//    public void LoadBuiltInText_ui(List<Dictionary<string, object>> rowList)
//    {
//        dtBuiltInText_ui = new Dictionary<string, Text_ui>();
//        foreach (var rowItem in rowList)
//        {
//            Text_ui dicItem = new Text_ui();
//            foreach (var item in rowItem)
//            {
//                var field = typeof(Text_ui).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                try { field.SetValue(dicItem, item.Value); }
//                catch { UnityEngine.Debug.LogError(item); }
//            }
//            if (dtBuiltInText_ui.ContainsKey(dicItem.index))
//                UnityEngine.Debug.LogError("Duplicate Key in Text_ui");
//            dtBuiltInText_ui.Add(dicItem.index, dicItem);
//        }
//    }
//    public Text_ui GetBuiltInText_uiData(string _index)
//    {
//        if (!dtBuiltInText_ui.ContainsKey(_index))
//        {
//            UnityEngine.Debug.LogError("Table Built in Text_ui");
//            UnityEngine.Debug.LogError(string.Format("table doesn't contain id {0}", _index));
//            return null;
//        }
//        return dtBuiltInText_ui[_index];
//    }
//    public Dictionary<string, Text_ui> GetBuiltInText_uiData()
//    {
//        return dtBuiltInText_ui;
//    }

//    #endregion

//    #region Gacha
//    public bool hasGachaByBoxid(int _boxID)
//    {
//        var targetItem = dtGacha_inform.Values.Where(item => item.box_id == _boxID).ToList();
//        return targetItem.Count > 0;
//    }
//    #endregion



//    #region Contest

//    public int GetContestReqPoint(int _season, int _step)
//    {
//        return dtContest_step_reward.Values.Where(item => item.season_index == _season && item.support_step == _step).First().req_support_pt;
//    }

//    public int GetContestReqStepPoint(int _season, int _step)
//    {
//        if (_step == 1)
//        {
//            return GetContestReqPoint(_season, _step);
//        }
//        else
//        {
//            int prevPoint = GetContestReqPoint(_season, _step - 1);
//            int currPoint = GetContestReqPoint(_season, _step);
//            return currPoint - prevPoint;
//        }
//    }
//    //public int GetContestReqPointAcc(int _season, int _step)
//    //{
//    //    int reqPoint = 0;
//    //    var contestList = dtContest_step_reward.Values.Where(item => item.season_index == _season && item.support_step <= _step).ToList();
//    //    foreach (var item in contestList)
//    //    {
//    //        reqPoint += item.req_support_pt;
//    //    }
//    //    return reqPoint;
//    //}

//    public int GetContestMaxStep(int _season)
//    {
//        var stepRewardList = dtContest_step_reward.Values.Where(item => item.season_index == _season).ToList();
//        return stepRewardList.Max(item2 => item2.support_step);
//    }

//    public Contest_step_reward GetContestStep(int _season, int _accPoint)
//    {
//        var stepRewardList = dtContest_step_reward.Values.Where(item => item.season_index == _season).ToList();
//        int maxStep = stepRewardList.Max(item2 => item2.support_step);
//        for (int i = 0; i < maxStep; i++)
//        {
//            if (_accPoint < stepRewardList[i].req_support_pt)
//            {
//                return stepRewardList[i];
//            }
//        }
//        return stepRewardList.Last(); 
//    }

//    //public Contest_step_reward GetContestStepFromAccPt(int _season, int _accPoint)
//    //{
//    //    var stepRewardList = dtContest_step_reward.Values.Where(item => item.season_index == _season).ToList();
//    //    int maxStep = stepRewardList.Max(item2 => item2.support_step);

//    //    for (int i = 0; i < maxStep; i++)
//    //    {   
//    //        int reqPoint = GetContestReqPointAcc(_season, i);
//    //        if (reqPoint > _accPoint)
//    //        {
//    //            return stepRewardList.Find(item => item.support_step == i);
//    //        }
//    //    }

//    //    return stepRewardList.Find(item => item.support_step == maxStep);
//    //}

//    #endregion

//}
//public static class DataTableExtention
//{
//    #region Myhouse

//    public static EQUIP_SLOT_TYPE GetVillagerEquipSlotType(this _Datatable.Villager villager, int _slotIndex)
//    {
//        var field = typeof(_Datatable.Villager).GetField($"equip_slot_type{_slotIndex + 1}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//        return (EQUIP_SLOT_TYPE)field.GetValue(villager);
//    }
//    public static int GetVillagerEquipOpen(this _Datatable.Villager villager, int _slotIndex)
//    {
//        var field = typeof(_Datatable.Villager).GetField($"equip_slot_open{_slotIndex + 1}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//        return (int)field.GetValue(villager);
//    }

//    public static int ConvertSlotTypeToIndex(this _Datatable.Villager villager, EQUIP_SLOT_TYPE _slotType, List<int> equipedList)
//    {
//        Dictionary<int, EQUIP_SLOT_TYPE> slotList = new Dictionary<int, EQUIP_SLOT_TYPE>();
//        for (int i = 0; i < 4; i++)
//        {
//            var field = typeof(_Datatable.Villager).GetField($"equip_slot_type{i + 1}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//            slotList.Add(i, (EQUIP_SLOT_TYPE)field.GetValue(villager));

//        }
//        var data = (from item in slotList
//                    where item.Value == _slotType
//                    select item).ToList();

//        foreach (var item in data)
//        {
//            if (!equipedList.Contains(item.Key))
//                return item.Key;
//        }

//        if (data.Count > 0)
//            return data.Last().Key;
//        else
//            return -1;
//    }
//    #endregion

//    #region Villager
//    public static int GetVillagerLastSkillID(this _Datatable.Villager villager)
//    {
//        for (int index = 4; index >= 1; index--)
//        {
//            var field = villager.GetType().GetField(string.Format("ability_skill_id{0}", index));
//            var skillId = (int)field.GetValue(villager);
//            if (skillId != 0)
//            {
//                return skillId;
//            }
//        }
//        return villager.ability_skill_id1;
//    }
//    #endregion
//    #region Rqst

//    public static int GetRequestMissionObjectID(this _Datatable.Request_mission _requestMission, int index)
//    {
//        switch (index)
//        {
//            case 0:
//                return _requestMission.req_mission_objid1;
//            case 1:
//                return _requestMission.req_mission_objid2;
//            case 2:
//                return _requestMission.req_mission_objid3;
//            case 3:
//                return _requestMission.req_mission_objid4;
//            case 4:
//                return _requestMission.req_mission_objid5;
//            case 5:
//                return _requestMission.req_mission_objid6;
//        }
//        return _requestMission.req_mission_objid1;
//    }
//    public static int GetRequestMissionObjectValue(this _Datatable.Request_mission _requestMission, int index)
//    {
//        switch (index)
//        {
//            case 0:
//                return _requestMission.req_mission_value1;
//            case 1:
//                return _requestMission.req_mission_value2;
//            case 2:
//                return _requestMission.req_mission_value3;
//            case 3:
//                return _requestMission.req_mission_value4;
//            case 4:
//                return _requestMission.req_mission_value5;
//            case 5:
//                return _requestMission.req_mission_value6;
//        }
//        return _requestMission.req_mission_value1;
//    }
//    #endregion


//    #region Structure
    
//    public static Dictionary<int, int> GetRemodelingRequiredItems(this _Datatable.Structure_remodeling _remodelingInfo)
//    {
//        Dictionary<int, int> retDic = new Dictionary<int, int>();
//        for (int i = 0; i < 4; i++)
//        {
//            var field = typeof(_Datatable.Structure_remodeling).GetField($"req_mtrl_id{i + 1}");
//            var reqItemID = (int)field.GetValue(_remodelingInfo);

//            field = typeof(_Datatable.Structure_remodeling).GetField($"req_mtrl_value{i + 1}");
//            var reqItemValue = (int)field.GetValue(_remodelingInfo);

//            if (reqItemID > 0)
//                retDic.Add(reqItemID, reqItemValue);
//        }
//        return retDic;
//    }
//    #endregion


//    #region Item

//    public static Dictionary<int, int> GetStorageUpgradeReqItems(this _Datatable.Item_storage storageInfo)
//    {
//        Dictionary<int, int> retDic = new Dictionary<int, int>();
//        for (int i = 0; i < 4; i++)
//        {
//            var field = typeof(_Datatable.Item_storage).GetField($"req_mtrl_id{i + 1}");
//            var reqItemID = (int)field.GetValue(storageInfo);

//            field = typeof(_Datatable.Item_storage).GetField($"req_mtrl_value{i + 1}");
//            var reqItemValue = (int)field.GetValue(storageInfo);

//            if (reqItemID > 0)
//                retDic.Add(reqItemID, reqItemValue);
//        }
//        return retDic;
//    }
//    public static Dictionary<int, int> GetStorageUpgradeReqItems(this _Datatable.Equip_storage storageInfo)
//    {
//        Dictionary<int, int> retDic = new Dictionary<int, int>();
//        for (int i = 0; i < 4; i++)
//        {
//            var field = typeof(_Datatable.Equip_storage).GetField($"req_mtrl_id{i + 1}");
//            var reqItemID = (int)field.GetValue(storageInfo);

//            field = typeof(_Datatable.Equip_storage).GetField($"req_mtrl_value{i + 1}");
//            var reqItemValue = (int)field.GetValue(storageInfo);

//            if (reqItemID > 0)
//                retDic.Add(reqItemID, reqItemValue);
//        }
//        return retDic;
//    }

//    public static _Datatable.Item GetItemBase(this _Datatable.Product productData)
//    {
//        return _Datatable.Instance.GetItemData(productData.item_id);
//    }
//    #endregion

//    #region Skill
//    public static int GetSkillValue1(this _Datatable.Villager_skill _skillInfo, int _level)
//    {
//        return _skillInfo.skill_fixed_ratio1 + (_level * _skillInfo.skill_level_ratio1);
//    }
//    public static float GetSkillEffect1(this _Datatable.Villager_skill _skillInfo, int _level, int baseVal = 0)
//    {
//        switch (_skillInfo.ratio_type1)
//        {
//            case RATIO_TYPE.FIX:
//                return baseVal + _skillInfo.GetSkillValue1(_level);
//            case RATIO_TYPE.RATE:
//                return baseVal + (baseVal * _skillInfo.GetSkillValue1(_level) * 0.001f);
//        }
//        return 0;
//    }

//    public static string GetSkillEffectDesc(this _Datatable.Villager_skill _skillInfo, int _level, int baseVal = 0)
//    {
//        // _skillInfo.ratio_type2가 있을경우, ratio_type2를 설명 인자값으로 보여준다.
//        if (_skillInfo.ratio_type2 != RATIO_TYPE.NONE)
//        {
//            float value = _skillInfo.GetSkillValue2(_level);
//            switch (_skillInfo.ratio_type2)
//            {
//                case RATIO_TYPE.FIX:
//                    return value.ToString();
//                case RATIO_TYPE.RATE:
//                    return string.Format("{0:0.0}%", value * 0.1f);
//            }
//        }
//        else
//        {
//            float value = _skillInfo.GetSkillValue1(_level);
//            switch (_skillInfo.ratio_type1)
//            {
//                case RATIO_TYPE.FIX:
//                    return value.ToString();
//                case RATIO_TYPE.RATE:
//                    return string.Format("{0:0.0}%", value * 0.1f);
//            }
//        }

//        return string.Empty;
//    }

//    public static int GetSkillValue2(this _Datatable.Villager_skill _skillInfo, int _level)
//    {
//        return _skillInfo.skill_fixed_ratio2 + (_level * _skillInfo.skill_level_ratio2);
//    }
//    public static float GetSkillEffect2(this _Datatable.Villager_skill _skillInfo, int _level, int baseVal = 0)
//    {
//        switch (_skillInfo.ratio_type2)
//        {
//            case RATIO_TYPE.FIX:
//                return baseVal + _skillInfo.GetSkillValue2(_level);
//            case RATIO_TYPE.RATE:
//                return baseVal + (baseVal * _skillInfo.GetSkillValue2(_level) * 0.001f);
//        }
//        return 0;
//    }
//    #endregion
}