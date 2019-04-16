using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chat;

public class Rotate4 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Chat.ChatClient.variable[0] = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.E))
            //transform.Rotate(new Vector3(0, 0, 10), Space.Self);
        //else
            transform.localEulerAngles = new Vector3(0, 0, -server.Demo.variable[0]);
    }
}
