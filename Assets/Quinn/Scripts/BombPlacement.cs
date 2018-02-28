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
        NPlaceBomb();
	}
    //can be used to place a bomb via unity events
    public void PlaceBombHere()
    {
        PlaceBomb();
    }
    //returns true if bomb was placed successfuly use radius to control explosion radius
    public bool PlaceBomb(float placeDist = 1f, float bombRadius = 1.5f)
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, placeDist);
        List<GameObject> placeable = new List<GameObject>();
        //for each thing i hit with the sphere
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "BombDropZone")
            {
                TriggerZone placeZone = hitColliders[i].GetComponent<TriggerZone>();
                BombDropZone dropZone = hitColliders[i].GetComponent<BombDropZone>();
                //check that the tile doesnt have a bomb and contains the object that would like to place a bomb there
                if (dropZone.hasBomb == false && placeZone.Contains(gameObject))
                {
                    placeable.Add(hitColliders[i].gameObject);
                }
            }
        }
        //check to see if i can place somewhere and decide where to place if multiple areas are valid
        if (placeable.Count > 0)
        {
            BombDropZone dropZone = placeable[0].GetComponent<BombDropZone>();
            float min = Vector3.Distance(placeable[0].gameObject.transform.position, gameObject.transform.position);
            foreach (GameObject zone in placeable)
            {
                float dist = Vector3.Distance(zone.gameObject.transform.position, gameObject.transform.position);
                if (min >= dist)
                {
                    min = dist;
                    dropZone = zone.GetComponent<BombDropZone>();
                }
            }
            GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
            bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
            bomb.GetComponent<Bomb>().Radius = bombRadius;
            dropZone.hasBomb = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public Transform NPlaceBomb(float placeDist = 1f, float bombRadius = 1.5f)
    {
        Transform retVal = gameObject.transform;
        float y = gameObject.transform.rotation.eulerAngles.y % 360;
        if (y % 90 != 0)
        {
            if (y % 90 < 45)
            {
                retVal.Rotate(0, -(y % 90), 0);
            }
            else
            {
                retVal.Rotate(0, y % 90, 0);
            }
        }
        //RaycastHit[] hit = Physics.RaycastAll(gameObject.transform.position, (transform.forward * placeDist));
        return retVal;
    }
}
