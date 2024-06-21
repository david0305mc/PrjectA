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

        public void OnClickSpawn()
        {
            if (startTile == null || endTile == null )
                return;

            MapTestMoveObj moveObj = Lean.Pool.LeanPool.Spawn(testMoveObjPrefab, mapCreator.ObjectField, false);
            moveObj.transform.position = startTile.transform.position;
            moveObj.InitData(mapCreator, startTile.X, startTile.Y, endTile.X, endTile.Y);
        }
    }
}

