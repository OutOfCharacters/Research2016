using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour
{

    Vector3 center;
    Vector3 end;
    float angle;
    float radius;
    float endAngle;
    float targetAngle;

    //public for debugging
    public Vector3 target;


    public float maxSpeed;
    public float maxForce;
    public float mass;

    Vector3 desiredVelocity;
    Vector3 steering;

    Vector3 steeringForce;
    Vector3 acceleration;
    Vector3 position;


    Rigidbody rb;
    //----------------- wander AI
    public Vector3 wanderCircleCenter;
    public float wanderCircleRadius;
    public float angleChange;
    float wanderAngle;
    public Vector3 displacementMarker;

    //---------------- Wall Stuff
    float counterForce;
    public Transform wall1;
    public Transform wall2;
    public Transform wall3;
    public Transform wall4;

    //----------------------- try agian
    public Vector3 CircleDistance;
    public float CircleRadius;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(1,0,1);
        wanderAngle = 0;
        counterForce = 0;
        displacementMarker = new Vector3(.001f,0,.001f);
    }

    void FixedUpdate()
    {
        Move();
        //lookAtTarget();
        //StartCoroutine(AdjustWallForce());
    }

    void Move()
    {
        //call the wander function
        steering = wander();
        //make it more realistic
        rb.AddForce(steering);
        

        //Debug.Log("The velocity is " + rb.velocity.ToString("F4"));
    }

    void lookAtTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(this.transform.forward - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
    }

    Vector3 wander()
    {
        //center of the circle is the velocity direction
        wanderCircleCenter = rb.velocity.normalized;
        wanderCircleCenter.y = 0;
        //Debug.Log("The normalized velocity is " + wanderCircleCenter.ToString("F4"));
        //scaled by the radius
        wanderCircleCenter = wanderCircleCenter * wanderCircleRadius;
        //Debug.Log("When it is scaled by the radius it is " + wanderCircleCenter.ToString("F4"));

        //Debug.Log("The current wander angle is " + wanderAngle);
        //Debug.Log("The quaternion is doing this thing " + Quaternion.AngleAxis(wanderAngle, Vector3.up).ToString("F4"));
        //displacement marker is the pervious displacement marker rotated by the wander angle
        displacementMarker = Quaternion.AngleAxis(wanderAngle, Vector3.up) * displacementMarker;
        Debug.Log("The displacement marker is " + displacementMarker.ToString("F4"));

        //wander angle is changed by a (random value between -.5 and .5 (walls counter force adjusts this)) * (angleChange)
        //angle change is set externally (as 1 by default)
        //counterForce is increased based on how close the spider gets to the wall.
        wanderAngle += Random.value * angleChange - angleChange * (.5f + counterForce);
        Vector3 wanderForce;
        wanderForce = wanderCircleCenter + displacementMarker;
        //Debug.Log("The wander force is " + wanderForce.ToString("F4"));

        return wanderForce;


    }
    Vector3 wanderTest()
    {
        Vector3 circleCenter;
        circleCenter = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        circleCenter.Normalize();
        circleCenter += CircleDistance;

        displacementMarker = new Vector3(0, 0, 1);
        displacementMarker *= CircleRadius;

        SetAngle(ref displacementMarker, wanderAngle);

        wanderAngle += (Random.value * angleChange) - (angleChange * .5f);

        Vector3 wanderForce = circleCenter + displacementMarker;
        return wanderForce;
    }

    void SetAngle(ref Vector3 vector, float angle)
    {
        float len = vector.magnitude;
        vector.x = Mathf.Cos(angle) * len;
        vector.z = Mathf.Sin(angle) * len;
    }

    IEnumerator AdjustWallForce()
    {
        while (true)
        {
            float distanceToTarget1 = Vector3.Distance(transform.position, wall1.position); //one for each wall, wall is target.position
            float distanceToTarget2 = Vector3.Distance(transform.position, wall2.position);
            float distanceToTarget3 = Vector3.Distance(transform.position, wall3.position);
            float distanceToTarget4 = Vector3.Distance(transform.position, wall4.position);

            //find each wall counter force
            float counterForce1 = .05f / distanceToTarget1;
            float counterForce2 = .05f / distanceToTarget2;
            float counterForce3 = .05f / distanceToTarget3;
            float counterForce4 = .05f / distanceToTarget4;


            counterForce = float.MaxValue;
            //Add all the forces to an arrayList
            ArrayList counterForces = new ArrayList();
            counterForces.Add(counterForce1);
            counterForces.Add(counterForce2);
            counterForces.Add(counterForce3);
            counterForces.Add(counterForce4);

            //find the lowest
            foreach (float cF in counterForces)
            {
                if (cF < counterForce)
                    //use the lowest value as the modifier to make the spider turn around more sharply
                    counterForce = cF;
            }
            //Debug.Log(counterForce);

            // change the counterForce once every second
            yield return new WaitForSeconds(1);
        }
    }




}