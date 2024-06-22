using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class BoidsTest2DManager : SingletonMono<BoidsTest2DManager>
    {
        
        [SerializeField] private Boids2DObj boidsObj;
        [SerializeField] private int objectCount;
        public List<Transform> WayPointList;

        public List<Boids2D> boidsObjList = new List<Boids2D>();

        private void Start()
        {
            
        }

        public void OnClickBtnSpawn()
        {

            for (int i = 0; i < 5; i++)
            {
                Boids2DObj obj = Lean.Pool.LeanPool.Spawn(boidsObj, transform, false);
                obj.transform.position = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
                //obj.transform.position = new Vector3(0, 0, 0);
                obj.WayPointList = WayPointList;
                obj.boidsObjList = boidsObjList;
                boidsObjList.Add(obj);
            }
            

        }

    }

}
