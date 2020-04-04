using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClick : MonoBehaviour
{
    //Set this in the inspector to add a function that will run if the button is clicked;
    public UnityEvent function;

    private bool pressed = false;
    public void ButtonAction(){
        if (!pressed){
            pressed = true;
            function.Invoke();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, transform.localPosition.z);
            StartCoroutine(ButtonUp());
        }
    }

    private IEnumerator ButtonUp(){
        yield return new WaitForSeconds(0.25f);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f, transform.localPosition.z);
        pressed = false;
    }

}
