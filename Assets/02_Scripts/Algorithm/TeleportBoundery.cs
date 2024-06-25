using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBoundery : MonoBehaviour
{
 
    private Boundary _boundary;

    private void Awake()
    {
        _boundary = new Boundary();
    }
    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > _boundary.XLimit)
        {
            if (transform.position.x > 0)
            {
                transform.position = new Vector3(-_boundary.XLimit, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(_boundary.XLimit, transform.position.y, transform.position.z);
            }
        }
        if (Mathf.Abs(transform.position.y) > _boundary.YLimit)
        {
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(transform.position.x, -_boundary.YLimit, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _boundary.YLimit, transform.position.z);
            }
        }
    }
}
