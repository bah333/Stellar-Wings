using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCollision : MonoBehaviour
{
    //Attach to objects that are able to press buttons.
    void OnCollisionEnter(Collision coll){
        ButtonClick script = coll.collider.GetComponent<ButtonClick>();
        if(script != null){
            script.ButtonAction();
        }
        
    }
}
