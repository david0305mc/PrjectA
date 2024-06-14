using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private void Awake()
    {
        //GameManager.Instance.StartGame();
    }

    private void Start()
    {
        foreach (var item in DataManager.Instance.UnitinfoArray)
        {
            Debug.Log($"item {item.id}");
        }

        Debug.Log($"ConfigTable.Instance.DefaultUnit01 {ConfigTable.Instance.DefaultUnit01}");

            
        
    }

}
