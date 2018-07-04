using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public int speed = 4;
    public int visionRange = 1;
    public int maxHealth = 10;

    private int health;
    private int knockbackTime = 0;

    private Transform playerTransform;

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        var targetHeading = playerTransform.position - transform.position;
        var targetDirection = targetHeading.normalized;

        if(knockbackTime == 0)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= visionRange)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    playerTransform.position, speed * Time.deltaTime);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsSwinging() &&
            Vector3.Distance(transform.position, playerTransform.position) < 1.5 &&
            knockbackTime == 0)
        {
            health -= 5;
            var direction = -(playerTransform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * 500);
            knockbackTime = 20;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (knockbackTime > 0)
            knockbackTime--;
	}
}
