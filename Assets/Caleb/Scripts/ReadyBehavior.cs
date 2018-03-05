using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyBehavior : MonoBehaviour {

    public GameObject[] ReadyUp;
    PlayerIndex pIdx = PlayerIndex.One;
    GamePadState state;
    public bool readyStart;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        int check = 0;

        for (int i = 0; i < ReadyUp.Length; i++)
        {
            if (ReadyUp[i].activeInHierarchy)
            {
                check++;
            }
        }

        if (check >= 2)
        {
            readyStart = true;
        }
        else
        {
            readyStart = false;
        }

        if (readyStart == true)
        {
            state = GamePad.GetState(pIdx);
            if (state.Buttons.Start == ButtonState.Pressed)
            {
                SceneManager.LoadScene("Level");
            }
        }
	}
}
