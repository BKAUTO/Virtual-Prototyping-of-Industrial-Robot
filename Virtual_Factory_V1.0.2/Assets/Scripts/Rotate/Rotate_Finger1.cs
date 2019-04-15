using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Finger1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
            transform.Rotate(new Vector3(0, 10, 0), Space.Self);
    }
}
