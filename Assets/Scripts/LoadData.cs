using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadData : MonoBehaviour {
    Text t;
    DataStorage ds;
    GameObject temp;
    
	// Use this for initialization
	void Start () {
        t = GetComponent<Text>();
        temp = GameObject.Find("Data Storage");
        ds = temp.GetComponent<DataStorage>();
        t.text = ds.GetStorage();
    }

}
