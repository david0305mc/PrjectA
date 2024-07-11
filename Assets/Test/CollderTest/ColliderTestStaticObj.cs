using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTestStaticObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{this.name} �� {collision.gameObject.name} ���� OnTriggerEnter2D �� �Ͼ");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{this.name} �� {collision.gameObject.name} ���� OnCollisionEnter2D �� �Ͼ");
    }
}
