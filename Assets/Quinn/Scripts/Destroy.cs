using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    private float timer = 0;
    public float duration = 0;
    public bool delayed = false;
    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
    public void DelayedDestroy(float delay)
    {
        delayed = true;
        duration = delay;
        timer = 0;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (delayed)
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                Destroy(gameObject);
                delayed = false;
                timer = 0;
            }
        }
	}
}
