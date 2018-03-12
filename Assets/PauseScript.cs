using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PauseScript : MonoBehaviour
{
    public Canvas pauseScreen;
    public PlayerIndex pIdx;
    GamePadState state;
    GamePadState prevState;

    public ControllerUX pausedPlayer;
    public Button resumeButton;
    public Button menuButton;
    public Button exitButton;
    public ColorBlock highlightColor;
	// Use this for initialization
	void Start ()
    {
     

    }



    // Update is called once per frame
    void Update ()
    {
        prevState = state;
        state = GamePad.GetState(pIdx);

		if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {

            if (Time.timeScale == 1)
            {
                resumeButton.colors = highlightColor;
                menuButton.colors = highlightColor;
                exitButton.colors = highlightColor;

                pausedPlayer.pIdx = pIdx;

                pauseScreen.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseScreen.gameObject.SetActive(false);
            }
        }
        if(Time.timeScale == 1)
        {
            pauseScreen.gameObject.SetActive(false);
        }
	}
}
