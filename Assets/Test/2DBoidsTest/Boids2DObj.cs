using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class Boids2DObj : MonoBehaviour
    {

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private bool isBoidsAlgorithm;
        public List<Transform> WayPointList;

        public float _weightforward;
        public float _weightCohesion;
        public float _weightSeparation;
        public float _weightAlignment;
        public int _viewRadius = 3;
        public int _turnSpeed = 3;
        public float _forwardSpeed = 3;


        private int targetIndex;

        private void FixedUpdate()
        {
            _rigidbody2D.MovePosition(CalculateVelocity());
            //Vector3 newPos = Vector3.MoveTowards(_rigidbody2D.position, endPoint, moveSpeed * Time.deltaTime);
            //rb.MovePosition(newPos);
            //_rigidbody2D.MovePosition(CalculateVelocity());
            FaceFront();
            //_rigidbody2D.SetRotation(CalculateVelocity());
        }

        private void Start()
        {
            targetIndex = 0;
        }

        Vector2 CalculateVelocity()
        {
            Transform target = GetNextTarget();
            if (isBoidsAlgorithm)
            {
                var velocity = CalculateBoidsAlgorithm(target);
                Vector2 newPos = Vector2.MoveTowards(_rigidbody2D.position, _rigidbody2D.position + velocity, Time.deltaTime * _forwardSpeed);
                //return CalculateBoidsAlgorithm(target);
                return newPos;
            }
            else
            {
                Vector2 newPos = Vector2.MoveTowards(_rigidbody2D.position, target.position, Time.deltaTime * _forwardSpeed);
                return newPos;
            }
        }
        Transform GetNextTarget()
        {
            var target = WayPointList[targetIndex];
            if (Vector2.Distance(target.position, transform.position) < 0.5f)
            {
                targetIndex++;
                if (targetIndex >= WayPointList.Count)
                {
                    targetIndex = 0;
                }
                target = WayPointList[targetIndex];
            }
            return target;
        }

        protected Vector2 CalculateBoidsAlgorithm(Transform target)
        {
            List<Boids2DObj> neighboringFish_list = GetNeighboringFishList();
            Vector2 targetVec = target.position - transform.position;

            //adding all velocity of all rules
            Vector2 velocity = (
                 //BoidsTest2DManager.Instance._weightforward * (Vector2)transform.right
                 _weightforward * targetVec
                + _weightCohesion * Rule1(neighboringFish_list)
                + _weightSeparation * Rule2(neighboringFish_list)
                + _weightAlignment * Rule3(neighboringFish_list)
            );
            return velocity;
        }
        //rotate the fish to current velocity
        void FaceFront()
        {
            float step = Time.fixedDeltaTime * _turnSpeed;
            Vector3 newDir = Vector3.RotateTowards(transform.right, _rigidbody2D.velocity, step, 0);

            float zOffset = Vector2.SignedAngle(transform.right, newDir);
            transform.Rotate(Vector3.forward, zOffset);
        }
        List<Boids2DObj> GetNeighboringFishList()
        {
            List<Boids2DObj> neighboringFish_list = new List<Boids2DObj>();

            //get neghboring fish
            foreach (var obj in BoidsTest2DManager.Instance.boidsObjList)
            {
                //don't include itself
                if (obj == this.gameObject) continue;

                if (Vector2.Distance(transform.position, obj.transform.position) <= _viewRadius)
                {
                    neighboringFish_list.Add(obj);
                }
            }
            return neighboringFish_list;
        }


        #region Rule1_Defination

        //Boid rule 1: Boids try to fly towards the centre of mass of neighbouring boids.
        //return the direction of this rule
        Vector2 Rule1(List<Boids2DObj> neighboringFish_list)
        {
            Vector2 direction = new Vector2();


            //get centrol position
            Vector2 centerPos = Vector2.zero;
            foreach (var fish in neighboringFish_list)
            {
                centerPos += (Vector2)fish.transform.position;
            }
            if (neighboringFish_list.Count != 0)
            {
                centerPos /= neighboringFish_list.Count;
            }
            else
            {
                centerPos = transform.position;
            }

            //get direction
            direction = (centerPos - (Vector2)this.transform.position).normalized;


            return direction;
        }

        #endregion

        #region Rule2_Defination
        //Rule 2: Boids try to keep a small distance away from other objects(including other boids).
        Vector2 Rule2(List<Boids2DObj> neighboringFish_list)
        {
            Vector2 direction = Vector2.zero;

            foreach (var fish in neighboringFish_list)
            {
                Vector2 awayFishVec = (Vector2)transform.position - (Vector2)fish.transform.position;
                //the closer the bigger weight it get
                float x = awayFishVec.magnitude / _viewRadius;
                float weight = 1;

                direction += awayFishVec.normalized * weight;
            }
            direction.Normalize();

            return direction;
        }
        #endregion

        #region Rule3_Defination
        Vector2 Rule3(List<Boids2DObj> neighboringFish_list)
        {
            Vector2 direction = new Vector2();

            Vector2 centrolVelocity = Vector2.zero;
            foreach (var fish in neighboringFish_list)
            {
                centrolVelocity += fish.GetComponent<Rigidbody2D>().velocity;
            }
            if (neighboringFish_list.Count != 0)
            {
                centrolVelocity /= neighboringFish_list.Count;
            }
            else
            {
                centrolVelocity = _rigidbody2D.velocity;
            }
            direction = centrolVelocity.normalized;

            return direction;
        }
        #endregion
    }

}
