using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlyPlaying : MonoBehaviour
{
  
    public GameObject player3;
    public GameObject player4;


    public RawImage player3Face;
    public RawImage player4Face;


    public Slider player3Slider;
    public Slider player4Slider;



    private void Start()
    {
        if (ReadyBehavior.players < 4)
        {
            player4.SetActive(false);
            player4Face.gameObject.SetActive(false);
            player4Slider.gameObject.SetActive(false);
        }

        if (ReadyBehavior.players < 3)
        {
            player3.SetActive(false);
            player3Face.gameObject.SetActive(false);
            player3Slider.gameObject.SetActive(false);
        }
    }
}
