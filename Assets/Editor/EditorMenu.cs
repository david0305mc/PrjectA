using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMenu
{

    [MenuItem("Assets/GenerateTableCode2")]
    public static void GenerateTableCode2()
    {
        if (EditorApplication.isPlaying) return;
        DataManager.GenDatatable();
        DataManager.GenConfigTable();
        DataManager.GenTableEnum();
        Debug.Log("GenerateTableCode2");
    }

    [MenuItem("Assets/GenerateTableCode")]
    public static void GenerateTableCode()
    {
        if (EditorApplication.isPlaying) return;
        DataManager.GenDatatable();
        DataManager.GenConfigTable();
        DataManager.GenTableEnum();
    }

    [MenuItem("Assets/GenerateTableEnum")]
    public static void GenerateTableEnum()
    {
        if (EditorApplication.isPlaying) return;
        DataManager.GenTableEnum();
        Debug.Log("GenerateTableEnum");
    }
    
    [MenuItem("Util/CleanCache")]
    public static void CleanCache()
    {
        if (Caching.ClearCache())
        {
            EditorUtility.DisplayDialog("알림", "캐시가 삭제되었습니다.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("오류", "캐시 삭제에 실패했습니다.", "확인");
        }
    }
    [MenuItem("Util/LocalizeKR")]
    public static void LocalizeKR()
    {
        //Localization.currLanguage = SUPPORT_LANGUAGE.KR;
    }
    [MenuItem("Util/LocalizeEN")]
    public static void LocalizeEN()
    {
        //Localization.currLanguage = SUPPORT_LANGUAGE.EN;
    }
    [MenuItem("Util/LocalizeJP")]
    public static void LocalizeJP()
    {
        //Localization.currLanguage = SUPPORT_LANGUAGE.JP;
    }
    
}
