using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bombTimer : MonoBehaviour
{
    public PlayerControl player;
    public Slider bombSlider;

	// Update is called once per frame
	void Update ()
    {
        bombSlider.maxValue = player.currentSpawnTimer;
        bombSlider.value = player.spawnTime;
        if (player.isAlive == false)
        {
            bombSlider.gameObject.SetActive(false);
        }
	}
}
