using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathIndicator : MonoBehaviour
{
    public PlayerControl player;

    public Texture playerDead;

    RawImage currentFace;

    Slider slid;
    // Use this for initialization
    void Start ()
    {
        currentFace = GetComponent<RawImage>();
        //player = GetComponent<PlayerControl>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (player.isAlive == false)
        {
            currentFace.texture = playerDead;
        }
	}
}
