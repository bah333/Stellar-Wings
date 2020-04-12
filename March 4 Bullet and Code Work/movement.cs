using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = -2f;
    // Start is called before the first frame update
    void Start()
    {
        float waitSeconds = 2f;
        float firstCall = 3f;
        InvokeRepeating("MoveCannon", firstCall, waitSeconds);
    }

    // Update is called once per frame
    void MoveCannon()
    {
        speed *= -1f;
        Rigidbody rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = new Vector3(0, 0, speed);
    }
}
