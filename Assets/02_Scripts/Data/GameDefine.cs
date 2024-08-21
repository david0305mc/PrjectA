using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDefine 
{
    public enum GameState
    {
        MainScene,
        InGame,
    }

    public static int layerUnit = 8;
    public static int LayerMaskUnit = 1 << layerUnit;
    public static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "SaveData");
    public static readonly int TestAttackDamage = 80;
    public static readonly float RandPosOffsetMin = -0.1f;
    public static readonly float RandPosOffsetMax = 0.1f;
    public static readonly int MyBossUnitTID = 20001;

    public static readonly int[,] OuterTile = {
        { 0, -1 },          //    U
        { 1, -1 },          // R, U
        { 1,  0 },          // R
        { 1, -1 },          // R, D
        { 0,  1 },          //    D
        {-1, -1 },          // L, D
        {-1,  0 },          // L
        {-1,  1 },          // L, U
    };
}

public class GamePath
{
    public static readonly string PopupPath = "Popup";
}

public class SceneName
{
    public static readonly string Splash = "0_Splash";
    public static readonly string Title = "1_Title";
    public static readonly string Loading = "2_Loading";
    public static readonly string Game = "3_Game";
}


public enum ELockState
{
    Locked,
    UnLocked,
}

public enum OSCode
{
    Unknown = -1,
    iOS = 0,
    Android = 1,
    Windows = Android,
}

public enum ETargetOS
{
    All = 1,
    iOS = 2,
    Android = 3
}

public enum EPlatform
{
    None,
    Google,
    Apple,
    Guest,
    Webus,
    DevWindows = 90,
    Unknown,
    Deleted,
}

public enum EServerStatus
{
    /// <summary>??????</summary>
    Live,
    /// <summary>????</summary>
    Review = 5,
    /// <summary>???????? ????</summary>
    Update_Recommend,
    /// <summary>???????? ????</summary>
    Update_Essential,
    /// <summary>????</summary>
    Maintenance,
}

public enum EBuildType
{
    Dev,
    Release,
}

public enum EServerType
{
    Dev,
    Qa,
    Review,
    Live,
}

public enum EItemType
{
    Heart = 1,
    Crystal = 2,
    Gem = 3,
    Gem_Free = 4,
    Gold_Crystal = 5,
    Heart_By_Once = 11, // ?????? 1?? ?????? ???? ?????? ?????????? ?? ?? ???????? ?????? type ??????. //http://172.20.102.178:8090/pages/viewpage.action?pageId=205095001
    Crystal_By_Once = 12, //?????? 1?? ?????? ???? ?????? ?????????? ?? ?? ???????? ?????? type ??????
    Food = 20,
    Fish = 30,
    CardPack = 40,
    Card = 50,
    Costume = 60,
    Dye = 61,
    Theme = 62,
    Ground = 70,
    ExpandItem = 71,
    IAP = 1000,			// ???? ?????? ????.
}

public enum EOwnerType
{
    None,
    Fish,
    Skill,
    Artifact,
    Coral,
    CardPack,
    Costume,
    Dye,
    Theme,
}

public enum EUnlockType
{
    NONE = 0,
    BUY_CORAL = 1,              //???? ???? ???? ???? -> ???? ?????? ????
    HAVE_FISH_KIND = 2,         //???? ???? ?????? ???? ???? -> ???? ?????? ????
    STONE_LEVELUP = 3,          //?????? ????    -> ?????? ???? ????
    HAVE_FISH = 4,              //?????? n ???? ????  -> ???? ?????? ????
    CORAL_LEVELUP = 5,          //???? ???? n ???? -> ???? ?????? ????
    ATTENDANCE_COUNT = 6,       //(???? ????) ???? n ?? ???? -> ???? 00?? ??????
    LOGIN_COUNT_PER_DAY = 7,    //(???? ????) ???? ?????? n?? ????   -> ???? 00?? ??????
    USE_SKILL = 8,              //(???? ????) ???? n ?? ???? ????
    SKILL_LEVELUP = 9,          //???? ???? -> ???? ?????? ????
    ARTIFACT_LEVELUP = 10,      //???????? ???? -> ???????? ?????? ????

    HAVE_FISH_SPECIES = 11,     //?????? n ?? -> ???? ?????? ????
    CARDPACK_OPEN = 12,         //(???? ????) ?????? ???? ???? ????
    CARDPACK_BUY = 13,          //(???? ????) ?????? ???? ???? ????
    HAVE_CARD = 14,             //???? n ?? ???? -> ???? ?????? ????
    HAVE_CARD_SEASON = 15,      //???? ???? n ?? ???? -> ???? ?????? ????
    DAILY_MISSION_CLEAR = 16,   //(???? ????) ???????? ?????? ???? ????
    CHANGE_THEME = 17,          //???? ???? ???? ???? ???? -> 2100 ???????????? ????
    CHANGE_COSTUME = 18,        //???? ???? ???? ?????? ???? -> 2100 ???????????? ????
    CHANGE_DYE = 19,            //???? ???? ???? ?????? ???? -> 2100 ???????????? ????
    AD_VIEW = 20,               //(???? ????) ???? ???? ???? ????

    BUY_IAP = 21,               //(???? ????) ???? ???? ???? ????
    CARD_COMPOSITE = 22,        //(???? ????) ???? ???? ????
    HAVE_CARD_GRADE_UR = 23,   // UR ?????? ?????? N?? ???? -> ???? ?????????? ????
    SET_FISH_KIND = 24,         //???? ?????? ???????? N???? ???? -> ???? ?????? ????

    USE_GEM = 80,               //(???? ????) ???? n ?? ???? ????
    USE_CRSYTAL = 81,           //(???? ????) ???? n ?? ???? ????
    USE_HEART = 82,             //(???? ????) ?????? n ?? ???? ????
    USE_CRAB_FOOD = 83,         //(???? ????) ?? ?????? n ?? ???? ????
    HARVEST_HEART = 84,         // ?????? ????????
    HARVEST_CRYSTAL = 85,       // ???? ????????

    WEMIX_LOGIN = 90,           //???? ???? -> 2100 ???????????? ????
    HAVE_NFT = 91,              //NFT ???????? -> NFT ?????????? ????
}

public class VersionData
{
    public int version;
    public OSCode os;
    public EServerStatus status;
    public string game_url;
    public string cdn_url;
    public int coupon_use;
}


public enum TileType
{
    Normal,
    Start,
    End,
    Block,
    Building,
}

public enum BuildingStates
{
    Drag,
    Idle,
    Attack,
}

public enum UnitStates
{
    UI,
    Idle,
    Move,
    Attack,
    End,
}

public enum UnitDataStates
{ 
    Alive,
    Dead,
}
