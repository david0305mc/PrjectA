using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTestStaticObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{this.name} 은 {collision.gameObject.name} 와의 OnTriggerEnter2D 가 일어남");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{this.name} 은 {collision.gameObject.name} 와의 OnCollisionEnter2D 가 일어남");
    }
}
