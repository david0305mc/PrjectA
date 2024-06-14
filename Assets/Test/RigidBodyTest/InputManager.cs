using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //[SerializeField] private Transform heroObject;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Rigidbody2D enemyRb2d;

    private float maxSpeed = 100f;
    private void FixedUpdate()
    {
        MoveHeroObject();
    }


    private void Update()
    {
        if (rb2d.velocity.magnitude > maxSpeed)
        {
            Debug.Log($"over Speed {rb2d.velocity.magnitude}");
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
        }
    }
    private void Awake()
    {

    }
    private void MoveHeroObject()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //rb2d.MovePosition(rb2d.position + (move * Time.deltaTime * 10f));
        rb2d.velocity = move * 30f;
        //rb2d.AddForce(move * Time.deltaTime * 1000f);
        enemyRb2d.AddForce((Vector2.right) * (Time.deltaTime * 10));
        //enemyRb2d.MovePosition(enemyRb2d.position + (Vector2.right) * (Time.deltaTime));
        //rb2d.MovePosition
        //heroObject.position += move;
    }
}
