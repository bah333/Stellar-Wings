using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // target -> position in front of player
    // public Transform targetInFront;
    GameObject targetInFront;
    // player -> actual player position
    //  public Transform player;
    GameObject player;

    Rigidbody rb;

    // Enemy movement speed
    public float speed;

    // Enemy max movement speed
    //  -> For AddForce implementation
    public float maxSpeed;

    // Distance where enemy switches focus from target to player and vice versa
    public float switchDistance;
    public float slowDownDistance;

    float targetDistance;
    float playerDistance;


    void Awake()
    {
        speed = 5.0f;
        maxSpeed = 10.0f;
        switchDistance = 20.0f;
        slowDownDistance = 10.0f;

        if (targetInFront == null)
            targetInFront = GameObject.FindGameObjectWithTag("targetInFront");


        if (player == null)
            player = GameObject.FindGameObjectWithTag("player");




        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        // Update distance from target and player each iteration
        targetDistance = Vector3.Distance(transform.position, targetInFront.transform.position);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);


        rb.AddForce(transform.forward * speed);

        // Max speed possible at any time
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (targetDistance >= switchDistance)
        {
            transform.LookAt(targetInFront.transform.position);
        }

        else
        {
            transform.LookAt(player.transform.position);
        }

        if (targetDistance <= slowDownDistance)
        {

            rb.velocity *= 0.9f;

            if (playerDistance <= 5.0f)
            {
                rb.AddForce(-transform.forward * speed);
            }

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5.0f);
            Debug.Log(playerDistance);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(this.gameObject);
    }
}
