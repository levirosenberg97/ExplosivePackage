using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum TriggerState
{
    Untriggered,
    Enter,
    Stay,
    Exit
}
public class TriggerZone : MonoBehaviour {

    //public 
    [Header("Objects with these tags will be collected when they collide with this object")]
    [Tooltip("leave empty or make an unused tag to not collect any objects")]
    public List<string> InteractsWithTags;
    [System.Serializable]
    public class MyEvent : UnityEvent { }
    public MyEvent OnEnter;
    public MyEvent OnStay;
    public MyEvent OnExit;
    //public bool 
    //private
    private List<GameObject> Entered = new List<GameObject>();
    private List<GameObject> Stayed = new List<GameObject>();
    private List<GameObject> Exited = new List<GameObject>();
    //used for other things to access the object that triggered it
    public List<GameObject> GetInteractors(TriggerState triggerState)
    {
        if (triggerState == TriggerState.Enter)
        {
            return Entered;
        }
        else if (triggerState == TriggerState.Stay)
        {
            return Stayed;
        }
        else if (triggerState == TriggerState.Exit)
        {
            return Exited;
        }
        else
        {
            Debug.Log(triggerState + " Not supported a triggerstate was passed\nNeeds to be enter stay or exit");
            return new List<GameObject>();
        }
    }
	// Update is called once per frame
	void Update () {
        if(Entered.Count > 0)
        {
            OnEnter.Invoke();
        }
        if (Stayed.Count > 0)
        {
            OnStay.Invoke();
        }
        if (Exited.Count > 0)
        {
            OnExit.Invoke();
        }
	}
    //clear the lists of objects 
    private void LateUpdate()
    {
        Entered = new List<GameObject>();
        Stayed = new List<GameObject>();
        Exited = new List<GameObject>();
    }
    //add to on enter
    void OnTriggerEnter(Collider other)
    {
        foreach (string tagToCheck in InteractsWithTags)
        {
            if (other.tag == tagToCheck)
            {
                Entered.Add(other.gameObject);
            }
        }
    }
    //add to inside
    void OnTriggerStay(Collider other)
    {
        //button or currently in
        foreach (string tagToCheck in InteractsWithTags)
        {
            if (other.tag == tagToCheck)
            {
                Stayed.Add(other.gameObject);
            }
        }
    }
    //add to on exit
    void OnTriggerExit(Collider other)
    {
        //exit
        foreach (string tagToCheck in InteractsWithTags)
        {
            if (other.tag == tagToCheck)
            {
                Exited.Add(other.gameObject);
            }
        }
    }
    //check if zone contians an object
    public bool Contains(GameObject search)
    {
        if (GetInteractors(TriggerState.Enter).Contains(search))
        {
            return true;
        }
        else if (GetInteractors(TriggerState.Stay).Contains(search))
        {
            return true;
        }
        else if (GetInteractors(TriggerState.Exit).Contains(search))
        {
            return true;
        }
        return false;
    }
}
