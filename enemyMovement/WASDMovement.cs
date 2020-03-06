using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{

    public float speed = 5.0f;
    public float maxSpeed = 10.0f;
    public float turnSpeed = 3.0f;
    public Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
        }


        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -turnSpeed, 0);
            // rb.AddTorque(transform.up * turnSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, turnSpeed, 0);
            //  rb.AddTorque(-transform.up * turnSpeed);
        }


        Vector3 slowDown = rb.velocity * -1;
        rb.AddForce(slowDown * .01f, ForceMode.Impulse);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
