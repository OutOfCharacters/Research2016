using UnityEngine;
using System.Collections;

public class WandMouse : MonoBehaviour
{
    //rigidbody of the mouse
    Rigidbody rb;
    //distance from center of circle to object
    public float circleDistance;
    //radius of circle
    public float radius;

    //position of circle
    public Vector3 circleCenter;

    //displacement vector for wander angle calculations
    Vector3 displacement = new Vector3(0, 0, 1);

    //wander angle
    float wanderAngle;
    //degree to change angle by
    public float angleChange;

    //------------------------------ 
    //walls to apply force
    public Transform wall1;
    public Transform wall2;
    public Transform wall3;
    public Transform wall4;

    //custom object which has counterforce magnitude and wall number values (wall number chooses what direction to apply force in
    counterForceObject counterForce;

    //the force we add to the object
    Vector3 forceToAdd;

    Vector3 counter1;
    Vector3 counter2;
    Vector3 counter3;
    Vector3 counter4;
    //----------------------------------

    // Use this for initialization
    void Start()
    {
       rb = GetComponent<Rigidbody>();

    }
    void ChangeAnimSpeed()
    {
        if (rb == null)
            Debug.Log("Rigidbody was null");
        else
        {
            Animator anim = GetComponent<Animator>();
            anim.speed = rb.velocity.magnitude * 8;
        }


    }


    void FixedUpdate()
    {
        wander();
        //WallForce();
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
        //Vector3 counterToAdd = addCounterForce();   //puts the force in the right direction
        //counterToAdd *= counterForce.getCF();       //adjusts the magnitude
        //counterToAdd.y = 0;
        //rb.AddForce(-counterToAdd);
        rb.AddForce(-counter1);
        rb.AddForce(-counter2);
        rb.AddForce(-counter3);
        rb.AddForce(-counter4);
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
                distanceToTarget1 = .05f;
            if (distanceToTarget2 < .05f)
                distanceToTarget2 = .05f;
            if (distanceToTarget3 < .05f)
                distanceToTarget3 = .05f;
            if (distanceToTarget4 < .05f)
                distanceToTarget4 = .05f;



            // (constant/ distance to target) = magnitude * (direction based on wall number) = vector from each wall
            counter1 = (.05f / distanceToTarget1) * addCounterForce(1);
            counter2 = (.05f / distanceToTarget2) * addCounterForce(2);
            counter3 = (.05f / distanceToTarget3) * addCounterForce(3);
            counter4 = (.05f / distanceToTarget4) * addCounterForce(4);

            // change the counterForce once every second
            yield return new WaitForSeconds(1);


        }
    }

    Vector3 addCounterForce(int wallNum)
    {
        //we will make a force opposite to the wallnumber (getWN) that the counterForce object has
        if (wallNum == 1)
            return new Vector3(1, 0, 0);
        else if (wallNum == 2)
            return new Vector3(-1, 0, 0);
        else if (wallNum == 3)
            return new Vector3(0, 0, 1);
        else if (wallNum == 4)
            return new Vector3(0, 0, -1);
        else //should not happen
            return new Vector3(0, 1, 0);
    }

    //Rotate to face the direction it's moving
    void RotateToVelocity(float turnSpeed)
    {
        Vector3 dir;
        //model not imported facing backwards so we do not set this to be negative so it faces the right way
        dir = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
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

    //Should account for contraints problems
    protected void LateUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0.02f, transform.localPosition.z);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

    }

}
