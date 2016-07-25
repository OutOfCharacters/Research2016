using UnityEngine;
using System.Collections;

public class AIM : MonoBehaviour
{
    public float fpsTargetDistance, enemyLookDistance, attackDistance, enemyMovementSpeed, damping;
    public MouseAI BAI;




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //look at target if they are within range
        //BAI.waypoint is fpsTarget
        fpsTargetDistance = Vector3.Distance(BAI.waypoint.position, transform.position);
        if (fpsTargetDistance < enemyLookDistance)
        {
            lookAtPlayer();
        }
    }

    //rotates object to look at enemy
    void lookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(BAI.waypoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

    }
}
