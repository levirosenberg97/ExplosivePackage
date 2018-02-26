using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour {
    public GameObject BombObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //returns true if bomb was placed successfuly use radius to control explosion radius
    public bool PlaceBomb(float radius = 1.5f)
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 1);
        int i = 0;
        List<GameObject> placeable = new List<GameObject>();
        //for each thing i hit with the sphere
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "BombDropZone")
            {
                TriggerZone placeZone = hitColliders[i].GetComponent<TriggerZone>();
                BombDropZone dropZone = hitColliders[i].GetComponent<BombDropZone>();
                if (dropZone.hasBomb == false && placeZone.Contains(gameObject))
                {
                    placeable.Add(hitColliders[i].gameObject);
                }
            }
            if(placeable.Count > 0)
            {
                BombDropZone dropZone = placeable[0].GetComponent<BombDropZone>();
                float min = Vector3.Distance(placeable[0].gameObject.transform.position, gameObject.transform.position);
                foreach (GameObject zone in placeable)
                {
                    float dist = Vector3.Distance(zone.gameObject.transform.position, gameObject.transform.position);
                    if (min > dist)
                    {
                        min = dist;
                        dropZone = zone.GetComponent<BombDropZone>();
                    }
                }
                GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
                bomb.GetComponent<Bomb>().Radius = radius;
                return true;
            }
        }
        return false;
    }
}
