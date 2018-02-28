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
    //can be used to place a bomb via unity events
    public void PlaceBombHere()
    {
        PlaceBomb();
    }
    //returns true if bomb was placed successfuly use radius to control explosion radius
    public bool PlaceBomb(float placeDist = 1f, float bombRadius = 1.5f)
    {
        Quaternion originalPos = gameObject.transform.rotation;
        Transform tempVal = gameObject.transform;
        //snap to nearest axis
        float y = gameObject.transform.rotation.eulerAngles.y % 360;
        if (y % 90 != 0)
        {
            if (y % 90 < 45)
            {
                tempVal.Rotate(0, -(y % 90), 0);
            }
            else
            {
                tempVal.Rotate(0, y % 90, 0);
            }
        }

        //set player rotation back
        gameObject.transform.rotation = originalPos;
        //see whats in that direction
        RaycastHit[] hit = Physics.RaycastAll(gameObject.transform.position, (tempVal.forward * placeDist), placeDist);
        List<GameObject> place = new List<GameObject>();
        float dist = placeDist + 1;
        foreach (RaycastHit hitInfo in hit)
        {
            if (hitInfo.collider.tag == "BombDropZone")
            {
                if (hitInfo.collider.gameObject.GetComponent<TriggerZone>().GetInteractors(TriggerState.All).Count == 0 && hitInfo.collider.gameObject.GetComponent<BombDropZone>().hasBomb == false)
                {
                    if (hitInfo.distance < dist)
                    {
                        dist = hitInfo.distance;
                        place.Clear();
                        place.Add(hitInfo.collider.gameObject);
                    }
                }
            }
        }
        if (dist != placeDist + 1 && place.Count > 0)
        {
            BombDropZone dropZone = place[0].GetComponent<BombDropZone>();
            GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
            bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
            bomb.GetComponent<Bomb>().Radius = bombRadius;
            dropZone.hasBomb = true;
            //placed bomb
            return true;
        }
        //return I didnt place
        return false;
        //OLD
        //Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, placeDist);
        //List<GameObject> placeable = new List<GameObject>();
        ////for each thing i hit with the sphere
        //for (int i = 0; i < hitColliders.Length; i++)
        //{
        //    if (hitColliders[i].tag == "BombDropZone")
        //    {
        //        TriggerZone placeZone = hitColliders[i].GetComponent<TriggerZone>();
        //        BombDropZone dropZone = hitColliders[i].GetComponent<BombDropZone>();
        //        //check that the tile doesnt have a bomb and contains the object that would like to place a bomb there
        //        if (dropZone.hasBomb == false && placeZone.Contains(gameObject))
        //        {
        //            placeable.Add(hitColliders[i].gameObject);
        //        }
        //    }
        //}
        ////check to see if i can place somewhere and decide where to place if multiple areas are valid
        //if (placeable.Count > 0)
        //{
        //    BombDropZone dropZone = placeable[0].GetComponent<BombDropZone>();
        //    float min = Vector3.Distance(placeable[0].gameObject.transform.position, gameObject.transform.position);
        //    foreach (GameObject zone in placeable)
        //    {
        //        float dist = Vector3.Distance(zone.gameObject.transform.position, gameObject.transform.position);
        //        if (min >= dist)
        //        {
        //            min = dist;
        //            dropZone = zone.GetComponent<BombDropZone>();
        //        }
        //    }
        //    GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
        //    bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
        //    bomb.GetComponent<Bomb>().Radius = bombRadius;
        //    dropZone.hasBomb = true;
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
}
