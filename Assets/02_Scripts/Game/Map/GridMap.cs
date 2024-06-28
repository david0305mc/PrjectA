using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{

    [SerializeField] private TileObject tilePrefab;
    [SerializeField] private Transform mapRoot;
    [SerializeField] private Transform objectField;
    public Transform ObjectField { get { return objectField; } }

    public int gridCol = 10;
    public int gridRow = 13;
    [SerializeField] private TileObject[,] tiles;
    [SerializeField] Camera myCamera;

    public TileObject[,] Tiles { get => tiles; }
    private float aspect;
    private float worldHeight;
    private float worldWidth;
    private float gridWidth;
    private float tileWidth;

    private void Awake()
    {
        if (myCamera == null)
            myCamera = Camera.main;
        InitDefaultData();
    }
    private void InitDefaultData()
    {
        aspect = (float)1080 / 1920;
        //aspect = (float)Screen.width / Screen.height;
        worldHeight = myCamera.orthographicSize * 2;
        worldWidth = worldHeight * aspect;
        gridWidth = worldWidth;
        tileWidth = gridWidth / gridCol;
        tiles = new TileObject[gridCol, gridRow];

        var mapObjs = GetComponentsInChildren<TileObject>();
        foreach (var item in mapObjs)
        {
            tiles[item.X, item.Y] = item;
        }
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


        for (int j = 0; j < gridRow; j++)
        {
            for (int i = 0; i < gridCol; i++)
            {

                //UnityEditor.PrefabUtility.InstantiatePrefab

                //var obj = Instantiate(tilePrefab, mapRoot);
                var obj = UnityEditor.PrefabUtility.InstantiatePrefab(tilePrefab, mapRoot) as TileObject;
                //var obj = test as MapTestObj;
                obj.transform.position = Node2Pos(i, j);
                obj.transform.localScale = new Vector3(tileWidth, tileWidth);
                obj.gameObject.name = $"obj {i}_{j} ";
                obj.X = i;
                obj.Y = j;
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
}