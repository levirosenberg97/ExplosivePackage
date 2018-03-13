using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyBehavior : MonoBehaviour {
    public static int players;
    public GameObject[] ReadyUp;
    PlayerIndex pIdx = PlayerIndex.One;
    GamePadState state;
    public bool readyStart;
    public string scene = "PlayerTestScene1";
    public GameObject startText;
    public AudioSource switchSound;
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
                players = check;
            }
        }

        if (check >= 2)
        {
            readyStart = true;
            startText.SetActive(true);
        }
        else
        {
            readyStart = false;
            startText.SetActive(false);
        }

        if (readyStart == true)
        {
            state = GamePad.GetState(pIdx);
            if (state.Buttons.Start == ButtonState.Pressed)
            {
                switchSound.Play();
                SceneManager.LoadScene(scene);
                
            }
        }
	}
}
