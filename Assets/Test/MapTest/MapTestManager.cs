using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class MapTestManager : MonoBehaviour
    {
        [SerializeField] private MapTestMoveObj testMoveObjPrefab;
        [SerializeField] private MapCreator mapCreator;
        [SerializeField] private MapTestObj startTile;
        [SerializeField] private MapTestObj endTile;


        private List<PathNode> path = new List<PathNode>();
        private AbsPathFinder jpsPathFinder;

        private void Start()
        {
            if (startTile == null || endTile == null)
            {
                Debug.LogError("not load");
                return;
            }
            InitJPS();
        }

        private void InitJPS()
        {
            jpsPathFinder = new JPSPathFinder(this);
            jpsPathFinder.SetNode2Pos(mapCreator.Node2Pos);
            
            jpsPathFinder.InitMap(mapCreator.gridCol, mapCreator.gridRow);
            //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
            //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);
            jpsPathFinder.SetStartNode(startTile.X, startTile.Y);
            jpsPathFinder.SetEndNode(endTile.X, endTile.Y);

            path = jpsPathFinder.FindPath();
        }

        public void OnClickSpawn()
        {
            if (startTile == null || endTile == null || path == null)
                return;

            MapTestMoveObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, mapCreator.ObjectField, false);
            moveObj.transform.position = startTile.transform.position;
            moveObj.InitData(path);
        }
    }
}

