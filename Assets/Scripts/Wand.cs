﻿using UnityEngine;
using System.Collections;

public class Wand : MonoBehaviour {
    
    Rigidbody rb;
    public float circleDistance;
    public float radius;

    public Vector3 circleCenter;
    
    Vector3 displacement = new Vector3(0, 0, 1);

    float wanderAngle;
    public float angleChange;

    //------------------------------
    public Transform wall1;
    public Transform wall2;
    public Transform wall3;
    public Transform wall4;

    counterForceObject counterForce;

    Vector3 forceToAdd;
    //----------------------------------

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        
	}
    void ChangeAnimSpeed()
    {
        Animator anim = GetComponent<Animator>();
        anim.speed = rb.velocity.magnitude * 10 ;
        
    }


    void FixedUpdate()
    {
        wander();
        WallForce();
        RotateToVelocity(50f);
        ChangeAnimSpeed();
    }

    void wander()
    {
        drawCircle();                               //makes a circle ahead of object
        drawDisplacement();                         //chooses a point somewhere on the circumference of the circle randomly
        changeWanderAngle();                        //changes the movement vector to be pointing towards that random point

  
        forceToAdd = drawWanderForce();             //sets the force to be of the correct magnitude and in the direction of the wander angle
        StartCoroutine(AdjustWallForce());          //Starts calculating the force so that the spider does not hit the wall 
        forceToAdd.y = 0;
        rb.AddForce(forceToAdd * 2f);               //adds the force
        
    }
    void WallForce()
    {
        Vector3 counterToAdd = addCounterForce();   //puts the force in the right direction
        counterToAdd *= counterForce.getCF();       //adjusts the magnitude
        counterToAdd.y = 0;
        rb.AddForce(-counterToAdd);
    }


    void drawCircle()
    {
        //makes a circle ahead of the object
        circleCenter = rb.velocity;
        circleCenter.Normalize();
        circleCenter *= circleDistance;
    }

    void drawDisplacement()
    {
        //finds a point on the circumference and draws a line between the center of the circle to that point
            displacement *= radius;
            displacement.z += circleDistance;
       
    }

    void changeWanderAngle()
    {
        //changes the angle to match the new displacement we just found
        SetAngle(ref displacement, wanderAngle);
        //adds (random value - .5f) (angleChange) angle change is set in inspector
        wanderAngle += (Random.value * angleChange) - (angleChange * .5f);
    }

    //helper method
    void SetAngle(ref Vector3 vector, float angle)
    {
        float len = vector.magnitude;
        vector.x = Mathf.Cos(angle) * len;
        vector.z = Mathf.Sin(angle) * len;
    }
    
    //return the force to be used for wandering
    Vector3 drawWanderForce()
    {
        Vector3 wanderForce;
        wanderForce = circleCenter + displacement;

        return wanderForce;
    }

    //add forces from walls
    IEnumerator AdjustWallForce()
    {
        while (true)
        {
            // one for each wall, wall is target.position
            float distanceToTarget1 = transform.localPosition.x - wall1.localPosition.x;
            float distanceToTarget2 = -transform.localPosition.x + wall2.localPosition.x;
            float distanceToTarget3 = transform.localPosition.z - wall3.localPosition.z;
            float distanceToTarget4 = -transform.localPosition.z + wall4.localPosition.z;



            if (distanceToTarget1 < .05f)
            {
                // if we get within .03 units of a wall
                distanceToTarget1 = .05f;
                //flip the wander force
                forceToAdd = -forceToAdd; 
            }
            if (distanceToTarget2 < .05f)
            {
                distanceToTarget2 = .05f;
                forceToAdd = -forceToAdd;
            }
            if (distanceToTarget3 < .05f)
            {
                distanceToTarget3 = .05f;
                forceToAdd = -forceToAdd;
            }

            if (distanceToTarget4 < .05f)
            {
                distanceToTarget4 = .05f;
                forceToAdd = -forceToAdd;
            }

            if (distanceToTarget1 > .1f) // if we are outside of .1 away from the wall
                distanceToTarget1 = float.MaxValue; //there is no wall force
            if (distanceToTarget2 > .1f)
                distanceToTarget2 = float.MaxValue;
            if (distanceToTarget3 > .1f)
                distanceToTarget3 = float.MaxValue;
            if (distanceToTarget4 > .1f)
                distanceToTarget4 = float.MaxValue;



            //find each wall counter force
            counterForceObject counterForce1 = new counterForceObject(.05f / distanceToTarget1, 1);
            counterForceObject counterForce2 = new counterForceObject(.05f / distanceToTarget2, 2);
            counterForceObject counterForce3 = new counterForceObject(.05f / distanceToTarget3, 3);
            counterForceObject counterForce4 = new counterForceObject(.05f / distanceToTarget4, 4);


            counterForce = new counterForceObject(float.MinValue, 0);

            //Add all the forces to an array
            counterForceObject[] arr = new counterForceObject[4];
            arr[0] = counterForce1;
            arr[1] = counterForce2;
            arr[2] = counterForce3;
            arr[3] = counterForce4;

            //find the lowest
            foreach (counterForceObject cF in arr)
            {
                if (cF.getCF() > counterForce.getCF())
                    //use the lowest value as the modifier to make the spider turn around more sharply
                    counterForce = cF;
            }
            
            // change the counterForce once every second
            yield return new WaitForSeconds(1);

            
        }
    }
    Vector3 addCounterForce()
    {
        //we will make a force opposite to the wallnumber (getWN) that the counterForce object has
        if (counterForce.getWN() == 1)
            return new Vector3(1, 0, 0);
        else if (counterForce.getWN() == 2)
            return new Vector3(-1, 0, 0);
        else if (counterForce.getWN() == 3)
            return new Vector3(0, 0, 1);
        else if (counterForce.getWN() == 4)
            return new Vector3(0, 0, -1);
        else //should not happen
            return new Vector3(0, 1, 0);
    }
    //Rotate to face the direction it's moving
    void RotateToVelocity(float turnSpeed)
    {
        Vector3 dir;
        //model imported facing backwards so we set this to be negative so it faces the right way
        dir = new Vector3(-rb.velocity.x, 0f, -rb.velocity.z);
        //don't rotate if the velocity is 0
        if (dir.magnitude > 0)
        {
            //dir.x = dir.x / 2;
            //dir.z = dir.z / 2;
            Quaternion dirQ = Quaternion.LookRotation(dir); 
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
            rb.MoveRotation(slerp);
        }
    }

    //decelerates the spider to 0, simulating normal stopping and starting movement of spiders

    void StopInPlace()
    {
        rb.AddForce(-rb.velocity, ForceMode.Impulse);
    }


}
