using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public GameObject originPoint;
    public GameObject Ship;
    public Rigidbody rb;
    public float thrust;    //multiplies the thrust vector by this amount
    public float thrustClamp;   //Clamp the thrust vector to this value
    public Vector3 thrustDirectionAdjustment = new Vector3(0.4f, 0.4f, 1.0f);//Reduce movement speed in non forward directions.
    float turnSpeed = 90f; //degrees per second, the maximum turn speed
    float maxTurnAngle = 65.0f; //degrees, angles greater than this will not increase the turn speed
    float minTurnAngle = 3.0f; //degrees, angles less than this will have no effect on ship turning;
    void Update () {
        //Calculate the difference between the steering object and its origin
        Vector3 positionDiff = transform.position - originPoint.transform.position;
        //Perform some operations to modify the  vector
        positionDiff = originPoint.transform.InverseTransformDirection(positionDiff);
        positionDiff = Vector3.ClampMagnitude(positionDiff,thrustClamp);
        positionDiff = Vector3.Scale(positionDiff, thrustDirectionAdjustment);
        //Add force to the ship based on the vector
        rb.AddRelativeForce(positionDiff * thrust);
        

        //Calculate the angle between the steering object and its origin
        float angle = Quaternion.Angle(transform.rotation, Ship.transform.rotation);
        //Perform some operations to determine if steering should be applied
        if (angle > minTurnAngle){
            if (angle > maxTurnAngle) angle = maxTurnAngle;
            
            float step = map (angle,0,maxTurnAngle,0,turnSpeed) * Time.deltaTime;
            //Turn the ship appropriately
            Ship.transform.rotation = Quaternion.RotateTowards(Ship.transform.rotation, transform.rotation, step);
        }        
    }

    //A simple mapping funciton used to map angles to turnspeeds
    float map(float value, float rangeOneLower, float rangeOneUpper, float rangeTwoLower, float rangeTwoUpper)
    {
        return rangeTwoLower + (value-rangeOneLower)*(rangeTwoUpper-rangeTwoLower)/(rangeOneUpper-rangeOneLower);
    }   

   
}
