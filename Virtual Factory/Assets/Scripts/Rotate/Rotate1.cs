using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chat;

public class Rotate1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Chat.ChatClient.variable[3] = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.B))
           //transform.Rotate(new Vector3(10, 0, 0), Space.Self); 
        //else
           transform.localEulerAngles = new Vector3(server.Demo.variable[3], 0, 0);
    }
}
