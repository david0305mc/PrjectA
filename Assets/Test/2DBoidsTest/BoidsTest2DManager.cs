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

        public List<Boids2DObj> boidsObjList = new List<Boids2DObj>();

        private void Start()
        {
            
        }

        public void OnClickBtnSpawn()
        {
            Boids2DObj obj = Lean.Pool.LeanPool.Spawn(boidsObj, transform, false);
            obj.transform.position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            obj.WayPointList = WayPointList;
            boidsObjList.Add(obj);

        }

    }

}
