using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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

    // float randomSpeedMin;
    //float randomSpeedMax;
    //float randomMaxSpeedMin;
    //  float randomMaxSpeedMax;
    float randomSpeed;
    float randomMaxSpeed;
    float randomOther;

    float timer;

    EnemyStats thisStats;

    void Awake()
    {
        switchDistance = 50.0f;
        slowDownDistance = 40.0f;

        randomSpeed = Random.Range(1.0f, 20.0f);
        randomMaxSpeed = Random.Range(randomSpeed + 2.0f, randomSpeed + 5.0f);
        randomOther = Random.Range(-2.0f, 2.0f);

        timer = 5.0f;

        if (targetInFront == null)
            targetInFront = GameObject.FindGameObjectWithTag("targetInFront");

        if (player == null)
            player = GameObject.FindGameObjectWithTag("player");

        rb = GetComponent<Rigidbody>();

        thisStats = this.GetComponent<EnemyStats>();
    }

    void Update()
    {
        // Update distance from target and player each iteration
        targetDistance = Vector3.Distance(transform.position, targetInFront.transform.position);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        timer -= Time.deltaTime;

        if (timer <= 0.0)
        {
            //randomOther = Random.Range(-10.0f, 10.0f);
            randomOther = Random.Range(-5.0f, 5.0f);
            timer = 5.0f;
        }

        // Ships have random acceleration speed
        rb.AddForce(transform.forward * randomSpeed);

        // "Floaty feel"
        rb.AddForce(transform.right * randomOther);
        rb.AddForce(transform.up * randomOther);


        // Max speed possible at any time
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, randomMaxSpeed);

        if (targetDistance >= switchDistance)
            transform.LookAt(targetInFront.transform.position);

        else
            transform.LookAt(player.transform.position);


        if (targetDistance <= slowDownDistance || playerDistance <= slowDownDistance)
        {
            thisStats.inRange = true;

            rb.velocity *= 0.9f;

            rb.AddForce(-transform.forward * randomSpeed);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5.0f);
        }
        else
            thisStats.inRange = false;

    }
}
