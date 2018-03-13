using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseStart : MonoBehaviour {

    public static PauseStart instance = null;
    public Canvas myCanvas;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        myCanvas = GetComponent<Canvas>();
    }
    void OnDestroy()
    {
        instance = null;
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
