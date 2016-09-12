using UnityEngine;
using System.Collections;

public class LSRPlaneCenter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 normal = -Camera.main.transform.forward;
        Vector3 pos = this.transform.position;

        UnityEngine.VR.WSA.HolographicSettings.SetFocusPointForFrame(pos, normal);
	}
}
