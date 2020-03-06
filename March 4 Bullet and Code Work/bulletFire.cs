using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFire : MonoBehaviour
{
    public GameObject Bullet;
    public float bulSpeed;
    // Start is called before the first frame update
    void Start()
    {
        float waitSeconds = 0.2f;
        float firstCall = 1.0f;
        InvokeRepeating("shootBullet", firstCall, waitSeconds);
    }

    // Update is called once per frame
    void shootBullet()
    {
        if(Input.GetKey("a")){
            GameObject newBul = Instantiate(Bullet, gameObject.transform);
            Rigidbody brb = newBul.GetComponent<Rigidbody>();
            brb.velocity = new Vector3(bulSpeed, 0, 0);
        }
    }
}
