using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class ControllerUX : MonoBehaviour {

    PlayerIndex pIdx = PlayerIndex.One;
    GamePadState state;
    GamePadState PrevState;
    public Button button;
    public Selectable selectable;
	// Use this for initialization
	void Start ()
    {
        selectable = button;
	}

    int downCount = 0;
    public bool getButtonDown()
    {

        if (state.Buttons.A == ButtonState.Pressed)
        {
            downCount++;
            if (downCount >= 1)
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
        PrevState = state;
        state = GamePad.GetState(pIdx);
        Vector3 Dir = Vector3.zero;

        if(PrevState.ThumbSticks.Left.Y == 0 && state.ThumbSticks.Left.Y != 0)
        {
            Dir.y = state.ThumbSticks.Left.Y;
        }

        if (PrevState.ThumbSticks.Left.X == 0 && state.ThumbSticks.Left.X != 0)
        {
            Dir.x = state.ThumbSticks.Left.X;
        }

        Selectable trySelect = button.FindSelectable(Dir);

        if (trySelect != null)
        {
            
            selectable = trySelect;
            button = (Button)selectable;
        }

        if (getButtonDown())
        {

            button.onClick.Invoke();
        }

        button.Select();
    }
}

