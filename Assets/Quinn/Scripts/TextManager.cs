using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
    public GameObject Player;
    public GameObject Invincible;
    public GameObject FireUp;
    public GameObject FireDown;
    public GameObject SpeedUp;
    public GameObject SpeedDown;
    public GameObject BombUp;
    public GameObject BombDown;
    public enum TextString
    {
        Invincible,
        FireUp,
        FireDown,
        SpeedUp,
        SpeedDown,
        BombUp,
        BombDown
    }

    public void SpawnText(TextString input)
    {
        if (input == TextString.Invincible)
        {
            GameObject spawned = Instantiate(Invincible);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.FireUp)
        {
            GameObject spawned = Instantiate(FireUp);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.FireDown)
        {
            GameObject spawned = Instantiate(FireDown);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.SpeedUp)
        {
            GameObject spawned = Instantiate(SpeedUp);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.SpeedDown)
        {
            GameObject spawned = Instantiate(SpeedDown);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.BombUp)
        {
            GameObject spawned = Instantiate(BombUp);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
        else if (input == TextString.BombDown)
        {
            GameObject spawned = Instantiate(BombDown);
            spawned.GetComponent<FollowPlayer>().Leader = Player;
        }
    }
}
