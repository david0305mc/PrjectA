using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform mapRoot;
    [SerializeField] private List<GameObject> tileList;

    [SerializeField] private int gridCol = 10;
    [SerializeField] private int gridRow = 13;
    [SerializeField] private GameObject[,] tiles;
    [SerializeField] Camera myCamera;
    private float aspect;
    private float worldHeight;
    private float worldWidth;
    private float gridWidth;
    private float tileWidth;
    
    private void InitDefaultData()
    {
        aspect = (float)1080 / 1920;
        //aspect = (float)Screen.width / Screen.height;
        worldHeight = myCamera.orthographicSize * 2;
        worldWidth = worldHeight * aspect;
        gridWidth = worldWidth;
        tileWidth = gridWidth / gridCol;
    }

    public Vector3 Node2Pos(int i, int j)
    {
        float gridHeigh = tileWidth * gridRow;

        float startX = -gridWidth / 2 + tileWidth / 2;
        float startY = -gridHeigh / 2 + tileWidth / 2;

        return new Vector3(startX + i * tileWidth, startY + j * tileWidth, 0);
    }
    [ContextMenu("Create Map")]
    private void CreateMap()
    {
        InitDefaultData();
        tiles = new GameObject[gridCol, gridRow];
        for (int i = 0; i < gridCol; i++)
        {
            for (int j = 0; j < gridRow; j++)
            {
                var obj = Instantiate(tilePrefab, mapRoot);
                obj.transform.position = Node2Pos(i, j);
                obj.transform.localScale = new Vector3(tileWidth, tileWidth);
                obj.gameObject.name = $"obj {i}_{j} ";
                tiles[i, j] = obj;

                //GameObject tileObj = Lean.Pool.LeanPool.Spawn(tilePrefab, mapRoot);
                //tileObj.transform.position = Node2Pos(i, j);
                //tileObj.transform.localScale = new Vector3(tileWidth, tileWidth);
                //tileObj.gameObject.name = $"obj {i}_{j} ";
                //tiles[i, j] = tileObj;
                //tileObj.InitData((node) =>
                //{
                //    OnNodeClick(node);
                //});
            }
        }
    }

    [ContextMenu("RemoveTilesAll")]
    private void RemoveTilesAll()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                DestroyImmediate(tiles[i, j]);
            }
        }
        tiles = null;
    }
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
        
        for (int i = 0; i < gridCol; i++)
        {
            for (int j = 0; j < gridRow; j++)
            {
                var obj = Instantiate(tilePrefab, mapRoot);
                obj.transform.position = new Vector3(i, j, 0);
                tileList.Add(obj);
            }
        }
    }
}
