using UnityEngine;
using System.Collections;

public class WandTest : MonoBehaviour
{
    //rigidbody of the mouse
    Rigidbody rb;
    //distance from center of sphere to object
    public float sphereDistance;
    //radius of circle
    public float radius;

    //position of circle
    public Vector3 sphereCenter;

    //displacement vector for wander angle calculations
    Vector3 displacement;

    //two wander angles, one for y and one for x,z
    float wanderAngle1;
    float wanderAngle2;


    //degree to change angles by
    public float angleChange;

    //walls from which we apply force
    public Transform wall1;
    public Transform wall2;
    public Transform wall3;
    public Transform wall4;
    public Transform wall5;
    public Transform wall6;


    //the force we add to the object
    Vector3 forceToAdd;

    Vector3 counter1;
    Vector3 counter2;
    Vector3 counter3;
    Vector3 counter4;
    Vector3 counter5;
    Vector3 counter6;

    //used to change animation blend tree
    Quaternion slerp;
    Quaternion dirQ;


    Animator anim;

    //declaring up here improves coroutine performance
    WaitForSeconds pointOne;

    //for the coroutine which adds wall force
    public bool notLevel3 = true;


    /// <summary>
    /// initializes the methods
    /// </summary>
    void Start()
    {

        //starts in a random direction(this will be the direction the mouse begins to move in)
        displacement = new Vector3(0, 0, 1);
        pointOne = new WaitForSeconds(.1f);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(AdjustWallForce());          //Starts calculating the force so that the spider does not hit the wall 
    }

    /// <summary>
    /// framerate independent update
    /// </summary>
    void FixedUpdate()
    {
        wander();
        WallForce();
        RotateToVelocity(100f);
        anim.speed = rb.velocity.magnitude * 10;
    }

    /// <summary>
    /// Calls the functions which make the wander force and adds it
    /// </summary>
    void wander()
    {
        drawCircle();
        forceToAdd = drawWanderForce();             //sets the force to be of the correct magnitude and in the direction of the wander angle
        rb.AddForce(-forceToAdd * 50f);             //adds the force

    }

    /// <summary>
    /// Adds the secondary wall force,
    /// used by fixedupdate,
    /// counter is set in "adjustwallforce"
    /// </summary>
    void WallForce()
    {
        rb.AddForce(counter1);
        rb.AddForce(counter2);
        rb.AddForce(counter3);
        rb.AddForce(counter4);
        rb.AddForce(counter5);
        rb.AddForce(counter6);
    }

    /// <summary>
    ///  draws the circle, sets the displacement and changes the wander angle for the next calculation
    /// </summary>
    void drawCircle()
    {
        //makes a circle ahead of the object
        sphereCenter = rb.velocity;
        sphereCenter.Normalize();
        sphereCenter *= sphereDistance;

        displacement *= radius;
        displacement.z += sphereDistance;

        //changes the angle to match the new displacement we just found
        SetAngle1(ref displacement, wanderAngle1);
        //adds (random value - .5f) (angleChange) ~~~~~~ angle change is set in inspector
        wanderAngle1 += (Random.value * angleChange) - (angleChange * .5f);
    }

    /// <summary>
    /// helper method for changeWanderAngle 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    void SetAngle1(ref Vector3 vector, float angle)
    {
        float len = vector.magnitude;
        vector.x = Mathf.Cos(angle) * len;
        vector.z = Mathf.Sin(angle) * len;
        vector.y = Mathf.Sin(angle) * len;
    }

    /// <summary>
    /// return the force to be used for wandering 
    /// </summary>
    /// <returns></returns>
    Vector3 drawWanderForce()
    {
        Vector3 wanderForce;
        wanderForce = sphereCenter + displacement;
        if (wanderForce.x > .10f)
            wanderForce.x = .10f;
        if (wanderForce.x < -.10f)
            wanderForce.x = -.10f;
        if (wanderForce.z < -.10f)
            wanderForce.z = -.10f;
        if (wanderForce.z > .10f)
            wanderForce.z = .10f;
        if (wanderForce.y > .10f)
            wanderForce.y = .10f;
        if (wanderForce.y > .10f)
            wanderForce.y = .10f;
        return wanderForce;
    }

    /// <summary>
    /// add forces from walls,
    /// sets checkForStall used in rorateToVelocity
    /// </summary>
    /// <returns></returns>
    IEnumerator AdjustWallForce()
    {
        while (notLevel3)
        {
            // one for each wall, wall is target.position
            float distanceToTarget1 = transform.localPosition.x - wall1.localPosition.x;
            float distanceToTarget2 = -transform.localPosition.x + wall2.localPosition.x;
            float distanceToTarget3 = transform.localPosition.z - wall3.localPosition.z;
            float distanceToTarget4 = -transform.localPosition.z + wall4.localPosition.z;
            float distanceToTarget5 = transform.localPosition.y - wall5.localPosition.y;
            float distanceToTarget6 = -transform.localPosition.y + wall6.localPosition.y;




            if (distanceToTarget1 < .02f)
                distanceToTarget1 = .02f;
            if (distanceToTarget2 < .02f)
                distanceToTarget2 = .02f;
            if (distanceToTarget3 < .02f)
                distanceToTarget3 = .02f;
            if (distanceToTarget4 < .02f)
                distanceToTarget4 = .02f;
            if (distanceToTarget5 < .02f)
                distanceToTarget5 = .02f;
            if (distanceToTarget6 < .02f)
                distanceToTarget6 = .02f;



            // (constant/ distance to target) = magnitude * (direction based on wall number) = vector from each wall
            counter1 = (.05f / distanceToTarget1) * wall1.transform.forward;
            counter2 = (.05f / distanceToTarget2) * wall2.transform.forward;
            counter3 = (.05f / distanceToTarget3) * wall3.transform.forward;
            counter4 = (.05f / distanceToTarget4) * wall4.transform.forward;
            counter5 = (.05f / distanceToTarget5) * wall5.transform.forward;
            counter6 = (.05f / distanceToTarget6) * wall6.transform.forward;


            // change the counterForce once every .1 seconds
            yield return pointOne;


        }
    }

    /// <summary>
    /// Rotate to face the direction it's moving
    /// </summary>
    /// <param name="turnSpeed"></param>
    void RotateToVelocity(float turnSpeed)
    {
        Vector3 dir;
        //model imported facing forwards so we do not set this to be negative like in the spider script
        dir = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

        //rotate if velocity is greater than 0
        if ((Mathf.Abs(dir.x) > 0 || Mathf.Abs(dir.z) > 0 || Mathf.Abs(dir.y) > 0))
        {
            //slerps to the direction the mouse is moving
            dirQ = Quaternion.LookRotation(dir);
            slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
            rb.MoveRotation(slerp);
        }


    }

}
