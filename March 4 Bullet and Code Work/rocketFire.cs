using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketFire : MonoBehaviour
{
    public GameObject Rocket;
    public float rocSpeed;
    // Start is called before the first frame update
    void Start()
    {
        float waitSeconds = 0.5f;
        float firstCall = 1.0f;
        InvokeRepeating("shootRocket", firstCall, waitSeconds);
    }

    // Update is called once per frame
    void shootRocket()
    {
        if(Input.GetKey("s")){
            GameObject newRoc = Instantiate(Rocket, gameObject.transform);
            Rigidbody rrb = newRoc.GetComponent<Rigidbody>();
            rrb.velocity = new Vector3(rocSpeed, 0, 0);
        }
    }
}
