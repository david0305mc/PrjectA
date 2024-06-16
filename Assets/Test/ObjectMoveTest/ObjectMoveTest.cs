using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace TEST
{
    public class ObjectMoveTest : MonoBehaviour
    {
        [SerializeField] private TileObj tileObjPrefab;
        [SerializeField] private GameObject floorRoot;
        [SerializeField] private Camera myCamera;

        private readonly int gridCol = 10;
        private readonly int gridRow = 13;

        private void Start()
        {
            float aspect = (float)Screen.width / Screen.height;
            
            float worldHeight = myCamera.orthographicSize * 2;
            float worldWidth = worldHeight * aspect;
            float gridWidth = worldWidth;
            float tileWidth = gridWidth / gridCol;
            float gridHeigh = tileWidth * gridRow;

            float startX = -gridWidth / 2 + tileWidth / 2;
            float startY = -gridHeigh / 2 + tileWidth / 2;

            //var tileObj = Lean.Pool.LeanPool.Spawn(tileObjPrefab, floorRoot.transform);
            //tileObj.transform.position = new Vector3(startX, startY, 0);
            //tileObj.transform.localScale = new Vector3(tileWidth, tileWidth);

            for (int i = 0; i < gridCol; i++)
            {
                for (int j = 0; j < gridRow; j++)
                {
                    var tileObj = Lean.Pool.LeanPool.Spawn(tileObjPrefab, floorRoot.transform);
                    tileObj.transform.position = new Vector3(startX + i * tileWidth, startY + j * tileWidth, 0);
                    tileObj.transform.localScale = new Vector3(tileWidth, tileWidth);
                }

            }


        }
    }


}

