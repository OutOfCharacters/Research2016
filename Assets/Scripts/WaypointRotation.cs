using UnityEngine;
using System.Collections;

public class WaypointRotation : MonoBehaviour {

    Camera main;
	// Use this for initialization
	void Start () {
        main = this.GetComponentInParent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(main.transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
