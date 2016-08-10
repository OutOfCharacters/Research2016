using UnityEngine;
using System.Collections;
using System;

public class MouseAI : MonoBehaviour

{

    public LevelState.State diff;

    public Transform[] waypointsBeginner;
    public Transform[] waypointsIntermediate;
    public Transform[] waypointsAdvanced;

    public Transform[] currentWaypoints;
    public int cWaypoint = 0;

    private Vector3 targetPos;
    public Vector3 moveDirection;

    public bool hitEnvironment; //did the mosue hit the wall?
    public Vector3 toReverse; //stores the random vector that the mouse was going in when it hit the wall
    public int counter;  //stores time to back off from the wall
    public int moveTime; //timetomove for mouse movement

    string tagLevel;
    public bool firstCollision;
    public bool checkForCollision;

    public Transform waypoint;



    // Use this for initialization
    void Start()
    {

        diff = LevelState.State.BEGINNER;
        currentWaypoints = waypointsBeginner;
        waypoint = currentWaypoints[0];
        currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
        counter = 0;
        moveTime = 0;
        hitEnvironment = false;
        firstCollision = true;
    }

    void FixedUpdate()
    {
        //when the mouse has not hit a wall
        if (counter == 0)
        {
            Move(.1f, moveTime);
        }
        //if the mouse has hit a wall, go in reverse mode for counter frames and then go forwards again
        else if (counter > 0)
        {
            Move(.2f, toReverse);
        }

    }

    void Move(float accel)
    {
        //Set target Position to waypoint position
        targetPos = currentWaypoints[cWaypoint].position;
        //stores the direction that the object was going when it hit the wall;
        toReverse = UnityEngine.Random.insideUnitSphere;
        toReverse.y = 0;
        //Set direction to move towards
        moveDirection = (targetPos - transform.position + toReverse * 1f).normalized;
        moveDirection.y = 0;
        //Move's the object
        transform.Translate(moveDirection * Time.deltaTime * accel, Space.World);
    }

    void Move(float accel, int timeToMove)
    {
        //if the mouse doesn't have a place to go
        if(timeToMove == 0)
        {
            //Set target Position to waypoint position
            targetPos = currentWaypoints[cWaypoint].position;


            //stores a new direction in a 45 degree darius of the previous direction
            float randx = UnityEngine.Random.Range(this.transform.forward.x - 5, this.transform.forward.x + 5);
            float randz = UnityEngine.Random.Range(this.transform.forward.z - 5, this.transform.forward.z + 5);
            toReverse = new Vector3(randx, 0, randz);
           
            //Set direction to move towards
            moveDirection = (targetPos - transform.position + toReverse * 1f).normalized;
            moveDirection.y = 0;
            //Move's the object
            transform.Translate(moveDirection * Time.deltaTime * accel, Space.World);
            moveTime = 100;
        }
        else
        {
            //Set target Position to waypoint position
            targetPos = currentWaypoints[cWaypoint].position;
            transform.Translate(moveDirection * Time.deltaTime * accel, Space.World);
            moveTime--;
        }
    }
    void Move(float accel, Vector3 reverseVector)
    {
        //Debug.Log(reverseVector);
        //Debug.Log(moveDirection);
        //moves the object the opposite direction of the wall
        transform.Translate((-reverseVector * 2f).normalized * Time.deltaTime * accel, Space.World);
        counter--;
    }


    /// <summary>
    /// enters a collider with a certain tag at a certain state
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        switch (diff)
        {
            //is at the beginner state
            case LevelState.State.BEGINNER:
                //if the collider entered is the waypoint we want, go to the next one
                //if the collider entered is any other way point, do nothing
                //if the collider entered is a wall, bounce off
                BeginnerState(other);
                break;
            case LevelState.State.INTERMEDIATE:
                IntermediateState(other);
                break;
            case LevelState.State.ADVANCED:
                AdvancedState(other);
                break;
        }
    }

    /// <summary>
    /// Handles the beginner level AI waypoints
    /// </summary>
    /// <param name="other"></param>
    void BeginnerState(Collider other)
    {
        if (other.gameObject.tag == "beginner current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "beginner removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {

                //start moving towards the next waypoint
                cWaypoint++;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
            }
            else
            {
                //start moving towards the next waypoint if you were at the last one
                cWaypoint = 0;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }


    /// <summary>
    /// Handles the Intermediate Level AI waypoints
    /// 
    /// </summary>
    /// <param name="other"></param>
    void IntermediateState(Collider other)
    {
        if (other.gameObject.tag == "intermediate current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "intermediate removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {
                //start moving towards the next waypoint
                cWaypoint++;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "intermediate current";
            }
            else
            {
                //start moving towards the next waypoint
                cWaypoint = 0;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "intermediate current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }
    /// <summary>
    /// Handles Adanved AI waypoints
    /// </summary>
    /// <param name="other"></param>
    void AdvancedState(Collider other)
    {
        if (other.gameObject.tag == "advanced current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "advanced removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {
                //start moving towards the next waypoint
                cWaypoint++;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "advanced current";
            }
            else
            {
                //start moving towards the next waypoint
                cWaypoint = 0;
                //set waypoint for AI to lookAt
                waypoint = currentWaypoints[cWaypoint];
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "advanced current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }
}
