using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ts();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void ts()
    {
        string go = "";
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            go = t.name;
        }
        Debug.Log(go);
    }
}
