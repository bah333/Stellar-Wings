using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // target -> position in front of player
    public Transform targetInFront;

    // player -> actual player position
    public Transform player;

    Rigidbody rb;

    // Enemy movement speed
    public float speed;

    // Enemy max movement speed
    //  -> For AddForce implementation
    public float maxSpeed;

    // Distance where enemy switches focus from target to player and vice versa
    public float switchDistance;

    float targetDistance;
    float playerDistance;

    void Awake()
    {
        speed = 4.0f;
        maxSpeed = 10.0f;
        switchDistance = 15.0f;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Update distance from target and player each iteration
        targetDistance = Vector3.Distance(transform.position, targetInFront.position);
        playerDistance = Vector3.Distance(transform.position, player.position);

        // If player distance from enemy is far away.
        //      -> Enemy will fly toward the front of player to "intercept"
        if (playerDistance >= switchDistance)
        {
            transform.LookAt(targetInFront);
            //   transform.position = Vector3.MoveTowards(transform.position, targetInFront.position, speed * Time.deltaTime);
            rb.AddForce(transform.forward * speed);
            Debug.Log("Imma get you");
        }

        // When player is close enough to enemy, will switch focus to player
        else
        {
            transform.LookAt(player);
            //   transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.AddForce(transform.forward * speed);
            Debug.Log("What up");
        }

        // Enforce max speed enemy can travel
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

    }

}
