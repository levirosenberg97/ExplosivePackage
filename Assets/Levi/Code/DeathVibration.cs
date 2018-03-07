using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class DeathVibration : MonoBehaviour
{
    public PlayerIndex playerNumber;
    public float vibrationCounter = 1;
    public PlayerControl player;
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
		if (player.isAlive == false && vibrationCounter > 0)
        {
            GamePad.SetVibration(playerNumber, 2f, 2f);
            vibrationCounter -= Time.deltaTime;
            if (vibrationCounter <= 0)
            {
                GamePad.SetVibration(playerNumber, 0f, 0f);
            }
        }
	}
}
