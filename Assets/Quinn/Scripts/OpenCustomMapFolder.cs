using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCustomMapFolder : MonoBehaviour {
    public void OpenMapFolder()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath + @"/Maps");
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
