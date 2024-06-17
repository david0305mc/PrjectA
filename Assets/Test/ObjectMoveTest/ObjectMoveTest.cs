using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FT;
using UnityEngine;

namespace TEST
{
    public class ObjectMoveTest : MonoBehaviour
    {
        public enum TileStatus
        {
            Normal,
            Block,
            Start,
            End,
            Path,
        }

        [SerializeField] private TileObj tileObjPrefab;
        [SerializeField] private GameObject floorRoot;
        [SerializeField] private Camera myCamera;

        [SerializeField] private GameObject blockOnObj;
        [SerializeField] private GameObject startOnObj;
        [SerializeField] private GameObject endOnObj;

        private TileStatus selectType;
        private readonly int gridCol = 10;
        private readonly int gridRow = 13;
        private AbsPathFinder jpsPathFinder;
        private TileObj[,] tiles;
        List<PathNode> path = new List<PathNode>();
        List<TileObj> displayList = new List<TileObj>();
        private TileObj startNode;
        private TileObj endNode;

        private void Start()
        {
            //jpsPathFinder = new JPSPathFinder(this);
            //jpsPathFinder.SetNode2Pos(Node2Pos);
            //jpsPathFinder.InitMap(WIDTH, HEIGHT);
            //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
            //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);
            InitMap();
        }
        public Vector3 Node2Pos(int i, int j)
        {
            float aspect = (float)Screen.width / Screen.height;

            float worldHeight = myCamera.orthographicSize * 2;
            float worldWidth = worldHeight * aspect;
            float gridWidth = worldWidth;
            float tileWidth = gridWidth / gridCol;
            float gridHeigh = tileWidth * gridRow;

            float startX = -gridWidth / 2 + tileWidth / 2;
            float startY = -gridHeigh / 2 + tileWidth / 2;

            return new Vector3(startX + i * tileWidth, startY + j * tileWidth, 0);
        }
        private void InitMap()
        {
            float aspect = (float)Screen.width / Screen.height;

            float worldHeight = myCamera.orthographicSize * 2;
            float worldWidth = worldHeight * aspect;
            float gridWidth = worldWidth;
            float tileWidth = gridWidth / gridCol;

            jpsPathFinder = new JPSPathFinder(this);
            jpsPathFinder.SetNode2Pos(Node2Pos);
            jpsPathFinder.InitMap(gridCol, gridRow);
            jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
            jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

            //var tileObj = Lean.Pool.LeanPool.Spawn(tileObjPrefab, floorRoot.transform);
            //tileObj.transform.position = new Vector3(startX, startY, 0);
            //tileObj.transform.localScale = new Vector3(tileWidth, tileWidth);
            tiles = new TileObj[gridCol, gridRow];
            for (int i = 0; i < gridCol; i++)
            {
                for (int j = 0; j < gridRow; j++)
                {
                    TileObj tileObj = Lean.Pool.LeanPool.Spawn(tileObjPrefab, floorRoot.transform);
                    tileObj.transform.position = Node2Pos(i, j);
                    tileObj.transform.localScale = new Vector3(tileWidth, tileWidth);
                    tileObj.gameObject.name = $"obj {i}_{j} ";
                    tileObj.X = i;
                    tileObj.Y = j;
                    tiles[i, j] = tileObj;
                    tileObj.InitData((node) => {
                        OnNodeClick(node);
                    });
                }
            }
        }

        private void DisplayRecord(PathNode node, Color color)
        {
            var tileObj = tiles[node.x, node.y];
            if (tileObj == startNode || tileObj == endNode)
                return;

            if (!displayList.Contains(tileObj))
                displayList.Add(tileObj);
            tileObj.SetColor(color);
        }

        private void OnNodeClick(TileObj node)
        {
            if (selectType == TileStatus.Block)
            {
                SetNodeBlock(node);
            }
            else if (selectType == TileStatus.Start)
            {
                SetNodeStart(node);
            }
            else if(selectType == TileStatus.End)
            {
                SetNodeEnd(node);
            }
        }

        private void SetNodeBlock(TileObj node)
        {
            if (node.status == TileStatus.Normal)
            {
                node.SetData(TileStatus.Block);
                jpsPathFinder.RefreshWalkable(node.X, node.Y, true);
            }
            else if (node.status == TileStatus.Block)
            {
                node.SetData(TileStatus.Normal);
                jpsPathFinder.RefreshWalkable(node.X, node.Y, false);
            }
        }

        private void SetNodeStart(TileObj node)
        {
            if (startNode != null)
            {
                startNode.SetData(TileStatus.Block);
            }
            startNode = node;
            startNode.SetData(TileStatus.Start);
        }

        private void SetNodeEnd(TileObj node)
        {
            if (endNode != null)
            {
                endNode.SetData(TileStatus.End);
            }
            endNode = node;
            endNode.SetData(TileStatus.End);
        }
        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    var ray = myCamera.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    var layerMask = 1 << LayerMask.NameToLayer("Tile");

            //    if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, layerMask))
            //    {
            //        Debug.Log(hit.collider.gameObject.name.ToString());
            //    }

            //}
        }

        public void OnClickAstarFind()
        {
            if (startNode != null && endNode != null)
            {
                OnClickClear();
                jpsPathFinder.SetStartNode(startNode.X, startNode.Y);
                jpsPathFinder.SetEndNode(endNode.X, endNode.Y);
                
                path = jpsPathFinder.FindPath();
            }
        }
        public void OnClickClear()
        {
            jpsPathFinder.Clear();

            foreach (var node in displayList)
                node.SetData(TileStatus.Normal);

            displayList.Clear();
        }

        private void OnPlayEnd()
        {
            foreach (var node in path)
            {
                var tileObj = tiles[node.x, node.y];
                tileObj.SetData(TileStatus.Path);
            }
        }

        public void OnClickJPSPath()
        {
            jpsPathFinder.FindPath();

        }
        public void OnClickBlock()
        {
            blockOnObj.SetActive(true);
            startOnObj.SetActive(false);
            endOnObj.SetActive(false);
            selectType = TileStatus.Block;
        }
        public void OnClickStart()
        {
            blockOnObj.SetActive(false);
            startOnObj.SetActive(true);
            endOnObj.SetActive(false);
            selectType = TileStatus.Start;
        }
        public void OnClickEnd()
        {
            blockOnObj.SetActive(false);
            startOnObj.SetActive(false);
            endOnObj.SetActive(true);
            selectType = TileStatus.End;
        }

    }


}

