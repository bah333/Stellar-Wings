using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public GameObject GrabbedObject;//The object currently in hand
    public GameObject CollidingObject;//An object that is within the hands grabbing reach
    public GameObject SteeringControl;
    public GameObject SteeringOrigin;
    public bool isLeft;

    void Update()
    {
        OVRInput.Update();
        if (isLeft){
            if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.3f && CollidingObject && GrabbedObject == null){
                Grab();
            }
            if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.3f && GrabbedObject)
            {
                Drop();
            }
        }else {
            if(OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.3f && CollidingObject && GrabbedObject == null){
                Grab();
            }
            if(OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.3f && GrabbedObject)
            {
                Drop();
            }
        }
    }

    public void OnTriggerEnter(Collider coll){
        //Update colliding Object if it isn't set, and only if the collision is with a non-kinematic object (to prevent grabbing the environment)
        if(CollidingObject == null && coll.gameObject == SteeringControl){//}) && coll.gameObject.GetComponent<Rigidbody>().isKinematic == false){
            CollidingObject = coll.gameObject;
        }
    }
    public void OnTriggerExit(Collider coll){
        if (coll.gameObject == CollidingObject){
            CollidingObject = null; 
        }
    }
    public void Grab()
    {
        GrabbedObject = CollidingObject;
        //Set grabbed objects parent to controller so it follows the controllers movement
        GrabbedObject.transform.SetParent (this.transform);
        //Disable physics interactions so the object stays in your hand.
        //GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Drop()
    {
        GrabbedObject.transform.SetParent (SteeringOrigin.transform);
        GrabbedObject.transform.localPosition = new Vector3(0,0,0); 
        GrabbedObject.transform.rotation = GrabbedObject.transform.parent.rotation;
        GrabbedObject = null;
        CollidingObject = null;      
    }
}
