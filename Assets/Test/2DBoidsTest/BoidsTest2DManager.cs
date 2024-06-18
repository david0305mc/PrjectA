using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class BoidsTest2DManager : SingletonMono<BoidsTest2DManager>
    {
        public List<Transform> WayPointList;
        [SerializeField] private Boids2DObj boidsObj;


        [SerializeField] private int objectCount;
        public float _weightforward;
        public float _weightCohesion;
        public float _weightSeparation;
        public float _weightAlignment;
        public int _viewRadius = 3;
        public int _turnSpeed = 3;
        public float _forwardSpeed = 3;

        public List<Boids2DObj> boidsObjList = new List<Boids2DObj>();

        private void Start()
        {
            for (int i = 0; i < objectCount; i++)
            {
                Boids2DObj obj = Lean.Pool.LeanPool.Spawn(boidsObj, transform, false);
                obj.transform.position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                boidsObjList.Add(obj);
            }
            
        }

    }

}
