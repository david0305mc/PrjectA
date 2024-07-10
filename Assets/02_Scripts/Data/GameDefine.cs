using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine 
{
    public static int layerUnit = 7;
    public static int LayerMaskUnit = 1 << layerUnit;
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
    /// <summary>라이브</summary>
    Live,
    /// <summary>리뷰</summary>
    Review = 5,
    /// <summary>업데이트 권장</summary>
    Update_Recommend,
    /// <summary>업데이트 강제</summary>
    Update_Essential,
    /// <summary>점검</summary>
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
    Heart_By_Once = 11, // 생명력 1회 생산량 만큼 보상을 지급하고자 할 때 사용하는 아이템 type 입니다. //http://172.20.102.178:8090/pages/viewpage.action?pageId=205095001
    Crystal_By_Once = 12, //생명력 1회 생산량 만큼 보상을 지급하고자 할 때 사용하는 아이템 type 입니다
    Food = 20,
    Fish = 30,
    CardPack = 40,
    Card = 50,
    Costume = 60,
    Dye = 61,
    Theme = 62,
    Ground = 70,
    ExpandItem = 71,
    IAP = 1000,			// 인앱 구매용 타입.
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
    BUY_CORAL = 1,              //특정 산호 보유 여부 -> 산호 리스트 체크
    HAVE_FISH_KIND = 2,         //특정 그룹 물고기 보유 여부 -> 피쉬 리스트 체크
    STONE_LEVELUP = 3,          //산호석 레벨    -> 산호석 레벨 체크
    HAVE_FISH = 4,              //물고기 n 마리 보유  -> 피쉬 리스트 체크
    CORAL_LEVELUP = 5,          //특정 산호 n 레벨 -> 산호 리스트 체크
    ATTENDANCE_COUNT = 6,       //(따로 기록) 출석 n 일 누적 -> 매일 00시 초기화
    LOGIN_COUNT_PER_DAY = 7,    //(따로 기록) 일일 로그인 n번 횟수   -> 매일 00시 초기화
    USE_SKILL = 8,              //(따로 기록) 스킬 n 회 사용 누적
    SKILL_LEVELUP = 9,          //스킬 레벨 -> 스킬 리스트 체크
    ARTIFACT_LEVELUP = 10,      //아티팩트 레벨 -> 아티팩트 리스트 체크

    HAVE_FISH_SPECIES = 11,     //물고기 n 종 -> 피쉬 리스트 체크
    CARDPACK_OPEN = 12,         //(따로 기록) 카드팩 오픈 횟수 누적
    CARDPACK_BUY = 13,          //(따로 기록) 카드팩 구매 횟수 누적
    HAVE_CARD = 14,             //카드 n 장 보유 -> 카드 리스트 체크
    HAVE_CARD_SEASON = 15,      //카드 시즌 n 장 보유 -> 카드 리스트 체크
    DAILY_MISSION_CLEAR = 16,   //(따로 기록) 일일미션 클리어 횟수 누적
    CHANGE_THEME = 17,          //현재 장착 중인 테마 체크 -> 2100 프로토콜에서 체크
    CHANGE_COSTUME = 18,        //현재 장착 중인 코스튬 체크 -> 2100 프로토콜에서 체크
    CHANGE_DYE = 19,            //현재 장착 중인 염색약 체크 -> 2100 프로토콜에서 체크
    AD_VIEW = 20,               //(따로 기록) 광고 시청 횟수 누적

    BUY_IAP = 21,               //(따로 기록) 인앱 구매 횟수 누적
    CARD_COMPOSITE = 22,        //(따로 기록) 합성 횟수 누적
    HAVE_CARD_GRADE_UR = 23,   // UR 등급의 카드를 N장 보유 -> 카드 리스트에서 체크
    SET_FISH_KIND = 24,         //특정 그룹의 물고기를 N마리 배치 -> 피쉬 리스트 체크

    USE_GEM = 80,               //(따로 기록) 보석 n 개 소모 누적
    USE_CRSYTAL = 81,           //(따로 기록) 결정 n 개 소모 누적
    USE_HEART = 82,             //(따로 기록) 생명력 n 개 소모 누적
    USE_CRAB_FOOD = 83,         //(따로 기록) 밥 아이템 n 개 소모 누적
    HARVEST_HEART = 84,         // 생명력 수확하기
    HARVEST_CRYSTAL = 85,       // 결정 수확하기

    WEMIX_LOGIN = 90,           //연동 여부 -> 2100 프로토콜에서 체크
    HAVE_NFT = 91,              //NFT 보유여부 -> NFT 리스트에서 체크
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
}

public enum UnitStates
{
    Drag,
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
