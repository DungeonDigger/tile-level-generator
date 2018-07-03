using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class Player : MonoBehaviour {

    public float speed = 4f;
    private Rigidbody2D rb2d;
    private bool canMove = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
            rb2d.velocity = targetVelocity * speed;
        }
        //else if(rb2d.velocity == Vector2.zero)
        //{
        //    rb2d.drag = 0;
        //    canMove = true;
        //}
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            canMove = false;
            //rb2d.velocity = -rb2d.velocity;
            Debug.Log("WOWO");
            rb2d.AddForce(Vector2.left * 100);
            rb2d.drag = 10;
        }
    }

}
