using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Manipulator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
            transform.Rotate(new Vector3(10, 0, 0), Space.Self);
    }
}
