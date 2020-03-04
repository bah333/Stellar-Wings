using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollide : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet" || 
        collision.gameObject.tag == "Rocket" || 
        collision.gameObject.tag == "Shrapnel"){
            Destroy(collision.gameObject);
        }
    }
}
