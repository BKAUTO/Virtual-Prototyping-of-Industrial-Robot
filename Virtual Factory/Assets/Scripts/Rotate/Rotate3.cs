using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chat;

public class Rotate3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Chat.ChatClient.variable[1] = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.D))
            //transform.Rotate(new Vector3(0, 10, 0), Space.Self);
        //else
            transform.localEulerAngles = new Vector3(0, -server.Demo.variable[1], 0);
    }
}
