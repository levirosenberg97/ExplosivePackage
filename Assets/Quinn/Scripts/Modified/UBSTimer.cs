using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class UBSTimer : MonoBehaviour
{
    public float Duration;
    public float CurrentTime;
    public bool Stopped = false;
    [System.Serializable]
    public class MyEvent : UnityEvent { }
    public MyEvent OnTimeUp;
    //stop timer (use reset to set the timer's time back to 0)
    public void StopTimer(bool reset = false)
    {
        if (reset)
        {
            ResetTime(true);
        }
        Stopped = true;
    }
    //used to start the timer (and to reset the timer)
    public void StartTimer(bool resetTimer)
    {
        if (resetTimer)
        {
            ResetTime(false);
        }
        Stopped = true;
    }
    //used to reset the time and stop the timer
    public void ResetTime(bool stopTimer = true)
    {
        if (stopTimer)
        {
            Stopped = true;
        }
        CurrentTime = 0;
    }
    //used to add time to the timer
    public void AddTime(float seconds)
    {
        Duration += seconds;
    }
    //used to get time left
    public float TimeLeft()
    {
        return Duration - CurrentTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Stopped == false)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= Duration)
            {
                OnTimeUp.Invoke();
            }
        }
    }
}