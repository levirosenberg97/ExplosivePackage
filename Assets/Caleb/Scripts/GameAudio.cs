using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAudio : MonoBehaviour {

    public AudioSource[] sound;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (!sound[0].isPlaying)
        {
            sound[1].Play();
        }
        else if (!sound[1].isPlaying)
        {
            sound[2].Play();
        }
        else if (!sound[2].isPlaying)
        {
            sound[0].Play();
        }
	}
}
