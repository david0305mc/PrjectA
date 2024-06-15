using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;

public class PathFinderTest : MonoBehaviour
{
    public enum SelectType
    {
        Block,
        Start,
        End,
    }

    const int WIDTH = 20;
    const int HEIGHT = 12;
    const int TILE_WIDTH = 100;
    const int TILE_HEIGHT = 100;

    [SerializeField] private TileObj tilePrefab;
    [SerializeField] private GameObject gridRoot;
    [SerializeField] private GameObject blockOnObj;
    [SerializeField] private GameObject startOnObj;
    [SerializeField] private GameObject endOnObj;

    TileObj[,] tiles;
    List<TileObj> displayList = new List<TileObj>();
    List<PathNode> path = new List<PathNode>();

    private SelectType selectType;
    private TileObj startNode;
    private TileObj endNode;
    private AbsPathFinder astarPathFinder;
    private AbsPathFinder jpsPathFinder;


    void Start()
    {
        astarPathFinder = new AStarPathFinder(this);
        astarPathFinder.SetNode2Pos(Node2Pos);
        astarPathFinder.InitMap(WIDTH, HEIGHT);
        astarPathFinder.recorder.SetDisplayAction(DisplayRecord);
        astarPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

        jpsPathFinder = new JPSPathFinder(this);
        jpsPathFinder.SetNode2Pos(Node2Pos);
        jpsPathFinder.InitMap(WIDTH, HEIGHT);
        jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

        InitMap();
        OnClickBlock();
    }


    public void OnClickJpsFind()
    {
        if (startNode != null && endNode != null)
        {
            OnClickClear();
            jpsPathFinder.SetStartNode(startNode.x, startNode.y);
            jpsPathFinder.SetEndNode(endNode.x, endNode.y);
            path = jpsPathFinder.FindPath();
        }
    }

    public void OnClickAstarFind()
    {
        if (startNode != null && endNode != null)
        {
            OnClickClear();
            astarPathFinder.SetStartNode(startNode.x, startNode.y);
            astarPathFinder.SetEndNode(endNode.x, endNode.y);
            path = astarPathFinder.FindPath();
        }
    }
    public void OnClickClear()
    {
        jpsPathFinder.Clear();

        foreach (var node in displayList)
            node.img.gameObject.SetActive(false);

        displayList.Clear();
    }
    private Vector3 Node2Pos(int i, int j)
    {
        return new Vector3(i * TILE_WIDTH, j * TILE_HEIGHT, 0);
    }

    private void DisplayRecord(PathNode node, Color color)
    {
        var tileObj = tiles[node.x, node.y];
        if (tileObj == startNode || tileObj == endNode)
            return;

        if (!displayList.Contains(tileObj))
            displayList.Add(tileObj);
        tileObj.img.gameObject.SetActive(true);
        tileObj.img.color = color;
    }

    private void OnPlayEnd()
    {
        foreach (var node in path)
        {
            var tileObj = tiles[node.x, node.y];
            tileObj.img.gameObject.SetActive(true);
            tileObj.img.color = Color.green;
        }
    }

    private void InitMap()
    {
        tiles = new TileObj[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                var node = Instantiate(tilePrefab, gridRoot.transform);
                node.x = i;
                node.y = j;
                node.transform.localPosition = Node2Pos(i, j);
                tiles[i, j] = node;
                node.button.onClick.AddListener(() =>
                {
                    OnNodeClick(node);
                });
            }
        }
        OnClickBlock();
    }

    private void OnNodeClick(TileObj node)
    {
        if (selectType == SelectType.Block)
        {
            SetNodeBlock(node);
        }
        else if (selectType == SelectType.Start)
        {
            SetNodeStart(node);
        }
        else
        {
            SetNodeEnd(node);
        }
    }

    private void SetNodeBlock(TileObj node)
    {
        if (node.img.gameObject.activeSelf)
        {
            node.img.gameObject.SetActive(false);

            astarPathFinder.RefreshWalkable(node.x, node.y, true);
            jpsPathFinder.RefreshWalkable(node.x, node.y, true);
        }
        else
        {
            node.img.gameObject.SetActive(true);
            node.img.color = Color.black;

            astarPathFinder.RefreshWalkable(node.x, node.y, false);
            jpsPathFinder.RefreshWalkable(node.x, node.y, false);
        }
    }
    private void SetNodeStart(TileObj node)
    {
        if (startNode != null)
        {
            startNode.img.gameObject.SetActive(false);
            startNode.button.interactable = true;
        }
        startNode = node;
        startNode.img.gameObject.SetActive(true);
        startNode.img.color = Color.green;
        startNode.button.interactable = false;
    }
    private void SetNodeEnd(TileObj node)
    {
        if (endNode != null)
        {
            endNode.img.gameObject.SetActive(false);
            endNode.button.interactable = true;
        }
        endNode = node;
        endNode.img.gameObject.SetActive(true);
        endNode.img.color = Color.red;
        endNode.button.interactable = false;
    }
    public void OnClickBlock()
    {
        blockOnObj.SetActive(true);
        startOnObj.SetActive(false);
        endOnObj.SetActive(false);
        selectType = SelectType.Block;
    }
    public void OnClickStart()
    {
        blockOnObj.SetActive(false);
        startOnObj.SetActive(true);
        endOnObj.SetActive(false);
        selectType = SelectType.Start;
    }
    public void OnClickEnd()
    {
        blockOnObj.SetActive(false);
        startOnObj.SetActive(false);
        endOnObj.SetActive(true);
        selectType = SelectType.End;
    }
}
