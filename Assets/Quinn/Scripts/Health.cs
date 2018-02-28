﻿using System.Collections;
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
    private BreakableWall wall;
    private bool isWall = false;
    private bool isPlayer = false;
    // Use this for initialization
    void Start()
    {
        CurrentHP = MaxHP;
        BreakableWall attempt = GetComponent<BreakableWall>();
        PlayerControl playerAttempt = GetComponent<PlayerControl>();
        if (attempt != null)
        {
            isWall = true;
            wall = attempt;
        }
        if (playerAttempt != null)
        {
            isPlayer = true;
            player = playerAttempt;
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
        if(isPlayer)
        {
            if (player.isInvincible == false)
            {
                {
                    CurrentHP = value;
                    if (CurrentHP <= 0)
                    {
                        if (isWall)
                        {
                            wall.Destroy();
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
        else
        {
            {
                CurrentHP = value;
                if (CurrentHP <= 0)
                {
                    if (isWall)
                    {
                        wall.Destroy();
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
}