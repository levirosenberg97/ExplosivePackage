using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class StartController : MonoBehaviour {

    public PlayerIndex pIdx;
    GamePadState state;
    public GameObject PlayerReady;
    // Use this for initialization
    void Start ()
    {
		
	}

    int downCount = 0;
    public bool getButtonDown()
    {

        if (state.Buttons.A == ButtonState.Pressed)
        {
            downCount++;
            if (downCount > 1)
            {
                return false;
            }
            return true;
        }
        downCount = 0;
        return false;
    }
    // Update is called once per frame
    void Update ()
    {
        state = GamePad.GetState(pIdx);
        if (getButtonDown() == true)
        {
            PlayerReady.SetActive(true);
        }
	}
}
