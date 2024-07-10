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
    /// <summary>���̺�</summary>
    Live,
    /// <summary>����</summary>
    Review = 5,
    /// <summary>������Ʈ ����</summary>
    Update_Recommend,
    /// <summary>������Ʈ ����</summary>
    Update_Essential,
    /// <summary>����</summary>
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
    Heart_By_Once = 11, // ����� 1ȸ ���귮 ��ŭ ������ �����ϰ��� �� �� ����ϴ� ������ type �Դϴ�. //http://172.20.102.178:8090/pages/viewpage.action?pageId=205095001
    Crystal_By_Once = 12, //����� 1ȸ ���귮 ��ŭ ������ �����ϰ��� �� �� ����ϴ� ������ type �Դϴ�
    Food = 20,
    Fish = 30,
    CardPack = 40,
    Card = 50,
    Costume = 60,
    Dye = 61,
    Theme = 62,
    Ground = 70,
    ExpandItem = 71,
    IAP = 1000,			// �ξ� ���ſ� Ÿ��.
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
    BUY_CORAL = 1,              //Ư�� ��ȣ ���� ���� -> ��ȣ ����Ʈ üũ
    HAVE_FISH_KIND = 2,         //Ư�� �׷� ����� ���� ���� -> �ǽ� ����Ʈ üũ
    STONE_LEVELUP = 3,          //��ȣ�� ����    -> ��ȣ�� ���� üũ
    HAVE_FISH = 4,              //����� n ���� ����  -> �ǽ� ����Ʈ üũ
    CORAL_LEVELUP = 5,          //Ư�� ��ȣ n ���� -> ��ȣ ����Ʈ üũ
    ATTENDANCE_COUNT = 6,       //(���� ���) �⼮ n �� ���� -> ���� 00�� �ʱ�ȭ
    LOGIN_COUNT_PER_DAY = 7,    //(���� ���) ���� �α��� n�� Ƚ��   -> ���� 00�� �ʱ�ȭ
    USE_SKILL = 8,              //(���� ���) ��ų n ȸ ��� ����
    SKILL_LEVELUP = 9,          //��ų ���� -> ��ų ����Ʈ üũ
    ARTIFACT_LEVELUP = 10,      //��Ƽ��Ʈ ���� -> ��Ƽ��Ʈ ����Ʈ üũ

    HAVE_FISH_SPECIES = 11,     //����� n �� -> �ǽ� ����Ʈ üũ
    CARDPACK_OPEN = 12,         //(���� ���) ī���� ���� Ƚ�� ����
    CARDPACK_BUY = 13,          //(���� ���) ī���� ���� Ƚ�� ����
    HAVE_CARD = 14,             //ī�� n �� ���� -> ī�� ����Ʈ üũ
    HAVE_CARD_SEASON = 15,      //ī�� ���� n �� ���� -> ī�� ����Ʈ üũ
    DAILY_MISSION_CLEAR = 16,   //(���� ���) ���Ϲ̼� Ŭ���� Ƚ�� ����
    CHANGE_THEME = 17,          //���� ���� ���� �׸� üũ -> 2100 �������ݿ��� üũ
    CHANGE_COSTUME = 18,        //���� ���� ���� �ڽ�Ƭ üũ -> 2100 �������ݿ��� üũ
    CHANGE_DYE = 19,            //���� ���� ���� ������ üũ -> 2100 �������ݿ��� üũ
    AD_VIEW = 20,               //(���� ���) ���� ��û Ƚ�� ����

    BUY_IAP = 21,               //(���� ���) �ξ� ���� Ƚ�� ����
    CARD_COMPOSITE = 22,        //(���� ���) �ռ� Ƚ�� ����
    HAVE_CARD_GRADE_UR = 23,   // UR ����� ī�带 N�� ���� -> ī�� ����Ʈ���� üũ
    SET_FISH_KIND = 24,         //Ư�� �׷��� ����⸦ N���� ��ġ -> �ǽ� ����Ʈ üũ

    USE_GEM = 80,               //(���� ���) ���� n �� �Ҹ� ����
    USE_CRSYTAL = 81,           //(���� ���) ���� n �� �Ҹ� ����
    USE_HEART = 82,             //(���� ���) ����� n �� �Ҹ� ����
    USE_CRAB_FOOD = 83,         //(���� ���) �� ������ n �� �Ҹ� ����
    HARVEST_HEART = 84,         // ����� ��Ȯ�ϱ�
    HARVEST_CRYSTAL = 85,       // ���� ��Ȯ�ϱ�

    WEMIX_LOGIN = 90,           //���� ���� -> 2100 �������ݿ��� üũ
    HAVE_NFT = 91,              //NFT �������� -> NFT ����Ʈ���� üũ
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
