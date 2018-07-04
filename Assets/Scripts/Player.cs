using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class Player : MonoBehaviour {

    public float speed = 4f;
    private Rigidbody2D rb2d;
    private Animator animator;
    private int knockbackCount = 0;
    private int swingCount = 0;

    [HideInInspector]
    public int health = 100;
    [HideInInspector]
    public int keyCount = 0;

    public bool IsSwinging()
    {
        return swingCount > 0;
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Sword"))
        {
            animator.SetTrigger("playerChop");
            swingCount = 10;
        }
        if (swingCount > 0)
            swingCount--;
    }

    void FixedUpdate()
    {
        if(knockbackCount == 0)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
            rb2d.velocity = targetVelocity * speed;
            if(targetVelocity.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            } else if(targetVelocity.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            knockbackCount--;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("enemyAttack");
            animator.SetTrigger("playerHit");
            knockbackCount = 10;
            var direction = -(collision.gameObject.transform.position - transform.position).normalized;
            rb2d.AddForce(direction * 500);
            health -= 10;
        }
        else if(collision.gameObject.CompareTag("Key"))
        {
            keyCount++;
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Door"))
        {
            if(keyCount > 0)
            {
                keyCount--;
                Destroy(collision.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("Treasure"))
        {
            DemoGameManager.instance.score += 100;
            Destroy(collision.gameObject);
        }
    }

}
