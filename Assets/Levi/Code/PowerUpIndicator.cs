using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpIndicator : MonoBehaviour
{
    public RawImage flame1;
    public RawImage flame2;
    public RawImage flame3;

    public Texture activated;
    public Texture deactivated;

    public PlayerControl player;
	// Update is called once per frame
	void Update ()
    {
		if (player.bombRadius < 2 )
        {
            flame1.texture = deactivated;
        }
        else if (player.bombRadius < 3)
        {
            flame2.texture = deactivated;
        }
        else if (player.bombRadius < 4)
        {
            flame3.texture = deactivated;
        }


        if (player.bombRadius == 2)
        {
            flame1.texture = activated;
        }
        else if (player.bombRadius == 3)
        {
            flame2.texture = activated;
        }
        else if (player.bombRadius == 4)
        {
            flame3.texture = activated;
        }


        if( player.isAlive == false)
        {
            gameObject.SetActive(false);
        }
	}
}
