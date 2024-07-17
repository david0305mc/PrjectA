using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids2D : MonoBehaviour
{

    // Component
    protected Rigidbody2D _rigidbody2D;

    [SerializeField] protected bool isBoidsAlgorithm;
    public List<Transform> WayPointList;

    public float _weightforward = 1;
    public float _weightCohesion = 1;
    public float _weightSeparation = 1;
    public float _weightAlignment = 0.1f;
    public float _viewRadius = 3;
    public int _turnSpeed = 3;
    public float _forwardSpeed = 3;

    public List<Boids2D> boidsObjList;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected Vector2 CalculateBoidsAlgorithm(Vector2 target)
    {
        List<Boids2D> neighboringFish_list = GetNeighboringFishList();
        Vector2 targetVec = target - (Vector2)transform.position;

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
    List<Boids2D> GetNeighboringFishList()
    {
        List<Boids2D> neighboringFish_list = new List<Boids2D>();

        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f, GameDefine.LayerMaskUnit);
        foreach (var collider in colliders)
        {
            if (collider == this.gameObject)
                continue;

            if (neighboringFish_list.Count >= 5)
                continue;

            var unitObj = collider.GetComponent<Boids2D>();
            if (unitObj != null)
            {
                neighboringFish_list.Add(unitObj);
            }
        }
        //get neghboring fish
        //foreach (var obj in boidsObjList)
        //{
        //    //don't include itself
        //    if (obj == this.gameObject) continue;

        //    if (Vector2.Distance(transform.position, obj.transform.position) <= _viewRadius)
        //    {
        //        neighboringFish_list.Add(obj);
        //    }
        //}
        return neighboringFish_list;
    }


    #region Rule1_Defination

    //Boid rule 1: Boids try to fly towards the centre of mass of neighbouring boids.
    //return the direction of this rule
    Vector2 Rule1(List<Boids2D> neighboringFish_list)
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
    Vector2 Rule2(List<Boids2D> neighboringFish_list)
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
    Vector2 Rule3(List<Boids2D> neighboringFish_list)
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
