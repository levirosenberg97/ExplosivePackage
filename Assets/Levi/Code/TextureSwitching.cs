using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureSwitching : MonoBehaviour
{
    public Texture Frame1;
    public Texture Frame2;
    public Texture Frame3;
    public Texture Frame4;
    public RawImage selection;
    float counter = 2f;

	// Use this for initialization

	
	// Update is called once per frame
	void Update ()
    {
        counter -= Time.deltaTime;

        if (counter >= 1.5f)
        {
            selection.texture = Frame1;
        }
        else if (counter >= 1f)
        {
            selection.texture = Frame2;
        }
        else if (counter >= .5f)
        {
            selection.texture = Frame3;
        }
        else
        {
            selection.texture = Frame4;
        }

        if (counter <= 0)
        {
            counter = 2f;
        }

	}
}
