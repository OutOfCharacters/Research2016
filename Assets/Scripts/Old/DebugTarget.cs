using UnityEngine;
using System.Collections;

public class DebugTarget : MonoBehaviour {
    public GameObject Spider;
    Vector3 newPos;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        newPos = Spider.GetComponent<WandSpider>().circleCenter;
        this.transform.localPosition = newPos;
	}
}
