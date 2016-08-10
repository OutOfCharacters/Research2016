using UnityEngine;
using System.Collections;

public interface IAIScripts
{


    void Move(float accel);

    void Move(float accel, int timeToMove);

    void Move(float accel, Vector3 reverseVector);

    void OnTriggerEnter(Collider other);

    void BeginnerState(Collider other);

    void IntermediateState(Collider other);

    void AdvancedState(Collider other);

    void GetcWaypoint();
    void setcWaypoint(int i);
    void GetCurrentWaypoints();
    void SetCurrentWaypoints(Transform[] t);
    void GetDiff();
    void SetDiff(LevelState.State s);
    void GetFirstCollisionState();
    void SetFirstCollisionState(bool b);
}
