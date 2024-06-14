using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
public class Articles_Search 
{
    // Actions Vars
    public List<String> HiddenColumns = new List<String>();
    public Dictionary<string, int> FormBehavior = new Dictionary<string, int>() { { "Control", 1 } }; 
}

public class CsvTestManager : MonoBehaviour
{
    public Dictionary<string, int> dicTest = new Dictionary<string, int>() { { "Control", 1 } };
    public void OnClickBtnTest()
    {
        //string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTzdUCZ3VJYDjTY8IJcv7lBXYoi_ek4ZYqslgNSY46FNEaBPiWnHytGT6kg7r0nxa0QTRYs1SaHRdYg/pub?gid=0&single=true&output=csv";
        //string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTfz02X_7HRFkZ5PmUKG4JlfKhic3VQaII6K_OqPfLlhEga-itVXdmLoPdq0WRM_0V3KcyR4t7YGfZ7/pub?gid=0&single=true&output=csv";
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTfz02X_7HRFkZ5PmUKG4JlfKhic3VQaII6K_OqPfLlhEga-itVXdmLoPdq0WRM_0V3KcyR4t7YGfZ7/pub?output=csv";

        UniTask.Create(async() => {

            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();
            Debug.Log($"www.downloadHandler.text {www.downloadHandler.text}");

        });

    }
    async UniTask Start()
    {
        //Debug.Log("UniTask Start");
        //await CheckLoadData();
        var itemPropertyInfo = dicTest.GetType().GetProperty("Item");
        itemPropertyInfo.SetValue(dicTest, 1, new[] { "test1" });

    }

    private void Awake()
    {
        //DataManager.Instance.Initialize();
        DataManagerTest.Instance.Initialize();
        DataManagerTest.Instance.LoadDataAsync();

    }
    public static List<string[]> MyConvert(List<object> mobj)
    {
        List<string[]> returnData = new List<string[]>();
        foreach (var item in mobj)
        {
            string[] arr = ((IEnumerable)item).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();
            returnData.Add(arr);
        }
        return returnData;
    }

    //public static List<string[]> MyConvert(List<object> mobj)
    //{
    //    List<string[]> returnData = new List<string[]>();
    //    foreach (var item in mobj)
    //    {
    //        string[] arr = ((IEnumerable)item).Cast<object>()
    //                         .Select(x => x.ToString())
    //                         .ToArray();
    //        returnData.Add(arr);
    //    }
    //    return returnData;
    //}
}
