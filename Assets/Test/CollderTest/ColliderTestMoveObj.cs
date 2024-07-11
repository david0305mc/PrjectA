using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTestMoveObj : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(-2, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{this.name} 은 {collision.gameObject.name} 와의 OnTriggerEnter2D 가 일어남");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{this.name} 은 {collision.gameObject.name} 와의 OnCollisionEnter2D 가 일어남");
    }

}
