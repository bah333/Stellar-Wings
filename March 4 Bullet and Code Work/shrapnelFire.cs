using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrapnelFire : MonoBehaviour
{
    public GameObject Shrapnel;
    public float shrSpeed;
    // Start is called before the first frame update
    void Start()
    {
        float waitSeconds = 1.0f;
        float firstCall = 1.0f;
        InvokeRepeating("shootShrapnel", firstCall, waitSeconds);
    }

    // Update is called once per frame
    void shootShrapnel()
    {
        if(Input.GetKey("d")){
            GameObject newShr1 = Instantiate(Shrapnel, gameObject.transform);
            Rigidbody srb1 = newShr1.GetComponent<Rigidbody>();
            srb1.velocity = new Vector3(shrSpeed, 0, 0);

            GameObject newShr2 = Instantiate(Shrapnel, gameObject.transform);
            Rigidbody srb2 = newShr2.GetComponent<Rigidbody>();
            srb2.velocity = new Vector3(shrSpeed, 0, 1f);

            GameObject newShr3 = Instantiate(Shrapnel, gameObject.transform);
            Rigidbody srb3 = newShr3.GetComponent<Rigidbody>();
            srb3.velocity = new Vector3(shrSpeed, 0, -1f);
        }
    }
}
