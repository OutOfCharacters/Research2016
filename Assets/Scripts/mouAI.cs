using UnityEngine;
using System.Collections;

public class mouAI : MonoBehaviour {
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
        anim.SetFloat("ForwardSpeed", .3f);
        anim.SetFloat("TurnSpeed", 1f);

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(gameObject.transform.position);
        //if (gameObject.transform.position.z > 2.1 || gameObject.transform.position.z < 1.9)
        //{
        //    anim.SetFloat("TurnSpeed", -1f);
        //    anim.SetFloat("ForwardSpeed", .5f);
        //}
        //else if (gameObject.transform.position.x > .1 || gameObject.transform.position.x < -.1)
        //{
        //    anim.SetFloat("TurnSpeed", -1f);
        //    anim.SetFloat("ForwardSpeed", .5f);
        //}
        //else
        //{
        //    anim.SetFloat("ForwardSpeed", .5f, .1f, Time.deltaTime);
        //    anim.SetFloat("TurnSpeed", 0f);
        //}

    }
}
