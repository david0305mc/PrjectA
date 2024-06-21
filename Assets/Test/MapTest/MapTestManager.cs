using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class MapTestManager : SingletonMono<MapTestManager>
    {
        [SerializeField] private MapTestMoveObj testMoveObjPrefab;
        [SerializeField] private MapCreator mapCreator;

        [SerializeField] private Vector2Int startPos;
        [SerializeField] private Vector2Int endPos;

        public List<MapTestMoveObj> mapMoveObjList = new List<MapTestMoveObj>();
        public void OnClickSpawn()
        {
            MapTestMoveObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, mapCreator.ObjectField, false);
            moveObj.InitData(mapCreator, startPos.x, startPos.y, endPos.x, endPos.y);
            mapMoveObjList.Add(moveObj);
        }
    }
}

