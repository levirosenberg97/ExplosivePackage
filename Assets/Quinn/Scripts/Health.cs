using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float CurrentHP = 1;
    public float MaxHP = 1;
    [System.Serializable]
    public class MyEvent : UnityEvent { }
    public MyEvent OnOverHeal;
    public MyEvent OnZeroHP;
    private PlayerControl player;
    private bool isWall = false;
    // Use this for initialization
    void Start()
    {
        CurrentHP = MaxHP;
        BreakableWall attempt = GetComponent<BreakableWall>();
        if (attempt != null)
        {
            isWall = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //used to deal damage (calls increment with a negitive value)
    public void TakeDamage(float value)
    {
        IncrementHP(-value);
    }

    //increments the value of current HP
    public void IncrementHP(float value)
    {
        SetHP(CurrentHP + value);
    }

    //sets the value of the HP bar
    public void SetHP(float value)
    {
        //if(player.isInvincible == false)
        {
            CurrentHP = value;
            if (CurrentHP <= 0)
            {
                if(isWall)
                {
                    GetComponent<BreakableWall>().Destroy();
                }
                else
                {
                    CurrentHP = 0;
                    OnZeroHP.Invoke();
                }
            }
            else if (CurrentHP > MaxHP)
            {
                OnOverHeal.Invoke();
                CurrentHP = MaxHP;
            }
        }
    }
}