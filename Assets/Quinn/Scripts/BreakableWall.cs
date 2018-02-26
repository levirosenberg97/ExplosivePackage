using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour {
    public GameObject dropArea;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Destroy()
    {
        Instantiate(dropArea, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
