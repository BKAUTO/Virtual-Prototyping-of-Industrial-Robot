using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chat;

public class Rotate2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Chat.ChatClient.variable[2] = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.C))
            //transform.Rotate(new Vector3(0, 10, 0), Space.Self);
        //else
            transform.localEulerAngles = new Vector3(0, -server.Demo.variable[2], 0);
    }
}
