using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 1; i <= 4; i++)
        {
            if (Input.GetAxis("Vertical" + i) != 0)
            {
                Debug.Log("P" + i + " Vertical input " + Input.GetAxis("Vertical" + i));
            }
            if (Input.GetAxis("Horizontal" + i) != 0)
            {
                Debug.Log("P" + i + " Horizontal input " + Input.GetAxis("Horizontal" + i));
            }
            if (Input.GetButton("Bomb" + i) == true)
            {
                Debug.Log("P" + i + " A BUTTON input == TRUE");
            }
        }
	}
}
