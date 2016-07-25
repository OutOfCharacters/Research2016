using UnityEngine;
using System.Collections;

public class WandTest : MonoBehaviour {
    
    Rigidbody rb;
    public float circleDistance;
    public float radius;

    public Vector3 circleCenter;
    
    Vector3 displacement = new Vector3(0, 0, 1);

    float wanderAngle;
    public float angleChange;
    //------------------------------

    float theta_scale = 0.01f;        //Set lower to add more points
    int size; //Total number of points in circle
    
    LineRenderer lineRenderer;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
	}

    void FixedUpdate()
    {
        wander();
    }

    void wander()
    {
        drawCircle();
        drawDisplacement();
        changeWanderAngle();
        Vector3 forceToAdd = drawWanderForce();
        rb.AddForce(forceToAdd);
    }

    void drawCircle()
    {

        circleCenter = Vector3.forward;
        circleCenter.Normalize();
        circleCenter *= circleDistance;
    }

    void drawDisplacement()
    {
            displacement *= radius;
            displacement.z += circleDistance;
        

        Debug.DrawLine(circleCenter, displacement, Color.white);
    }

    void changeWanderAngle()
    {
        SetAngle(ref displacement, wanderAngle);
        wanderAngle += (Random.value * angleChange) - (angleChange * .5f);
    }

    //helper method
    void SetAngle(ref Vector3 vector, float angle)
    {
        float len = vector.magnitude;
        vector.x = Mathf.Cos(angle) * len;
        vector.z = Mathf.Sin(angle) * len;
    }

    Vector3 drawWanderForce()
    {
        Vector3 wanderForce;
        wanderForce = circleCenter + displacement;
        
        
        
        Debug.DrawLine(Vector3.zero, wanderForce, Color.yellow);
        return wanderForce;
    }

}
