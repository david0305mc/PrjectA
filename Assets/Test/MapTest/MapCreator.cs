using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private int tileColumn;
    [SerializeField] private int tileRow;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform mapRoot;
    [SerializeField] private List<GameObject> tileList;
    //private void Start()
    //{
    //    DoCreatePrefab();
    //}

    [ContextMenu("Create Prefab From Selection")]
    void DoCreatePrefab()
    {
        if (tileList != null)
        {
            foreach (var item in tileList)
            {
                DestroyImmediate(item.gameObject);
            }
        }
        tileList = new List<GameObject>();
        mapRoot.transform.DetachChildren();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < tileRow; j++)
            {
                var obj = Instantiate(tilePrefab, mapRoot);
                obj.transform.position = new Vector3(i, j, 0);
                tileList.Add(obj);
            }
        }
    }
}
