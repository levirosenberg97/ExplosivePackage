﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour {
    public GameObject BombObject;
    public GameObject Indicator;
    private List<GameObject> Indicators;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }
    //can be used to place a bomb via unity events
    public void PlaceBombHere()
    {
        PlaceBomb();
    }
    public enum PlaceDir
    {
        ObjectDirection,
        Forward,
        Backward,
        Left,
        Right
    };
    //returns true if bomb was placed successfuly use radius to control explosion radius
    public bool PlaceBomb(float placeDist = 1.5f, float bombRadius = 1.5f, PlaceDir directionOverwrite = PlaceDir.ObjectDirection)
    {
        //adjust height to match boxes
        Vector3 adjustment = transform.position;
        adjustment.y = 0.5f;
        Vector3 rayDest = adjustment;
        //chose direction
        if (directionOverwrite == PlaceDir.ObjectDirection)
        {
            //find where the y rotation is closest to
            Vector3 snap = transform.eulerAngles;
            snap.y = Mathf.Round(snap.y / 90) * 90;
            if (snap.y % 360 == 0)
            {
                directionOverwrite = PlaceDir.Forward;
            }
            else if (snap.y % 360 == 90)
            {
                directionOverwrite = PlaceDir.Right;
            }
            else if (snap.y % 360 == 180)
            {
                directionOverwrite = PlaceDir.Backward;
            }
            else if (snap.y % 360 == 270)
            {
                directionOverwrite = PlaceDir.Left;
            }
        }
        // set destination based on direction
        if (directionOverwrite == PlaceDir.Forward)
        {
            //raycast forward/+z
            rayDest.z += placeDist;
        }
        else if (directionOverwrite == PlaceDir.Backward)
        {
            //raycast backward/-z
            rayDest.z -= placeDist;
        }
        else if (directionOverwrite == PlaceDir.Left)
        {
            //raycast left/-x
            rayDest.x -= placeDist;
        }
        else if (directionOverwrite == PlaceDir.Right)
        {
            //raycast right/+z
            rayDest.x += placeDist;
        }

        Debug.DrawLine(adjustment, rayDest, Color.red);
        RaycastHit hitInfo;
        if (Physics.Linecast(adjustment, rayDest, out hitInfo))
        {
            if (hitInfo.collider.tag == "BombDropZone")
            {
                if (hitInfo.collider.GetComponent<BombDropZone>().hasBomb == false)
                {
                    bool hasPlayer = false;
                    Collider[] inBombsPlace = Physics.OverlapSphere(hitInfo.collider.transform.position, 0.5f);
                    foreach (Collider col in inBombsPlace)
                    {
                        //player within bomb if spawned 
                        if (col.tag == "Player")
                        {
                            //skip the spawn
                            hasPlayer = true;
                            break;
                        }
                    }
                    if (hasPlayer == false)
                    {
                        BombDropZone dropZone = hitInfo.collider.gameObject.GetComponent<BombDropZone>();
                        GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
                        bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
                        bomb.GetComponent<Bomb>().Radius = bombRadius;
                        dropZone.hasBomb = true;
                        //placed bomb
                        return true;
                    }
                }
            }
        }
        return false;

        //RaycastHit[] hit = Physics.RaycastAll(adjustment, rayDest, placeDist);
        //List<GameObject> place = new List<GameObject>();
        //float dist = placeDist + 1;

        //foreach (RaycastHit hitInfo in hit)
        //{
        //    if (hitInfo.collider.tag == "BombDropZone")
        //    {
        //        if (hitInfo.collider.gameObject.GetComponent<BombDropZone>().hasBomb == false)
        //        {
        //            bool hasPlayer = false;
        //            Collider[] inBombsPlace = Physics.OverlapSphere(hitInfo.collider.transform.position, 0.5f);
        //            foreach(Collider col in inBombsPlace)
        //            {
        //                //player within bomb if spawned 
        //                if (col.tag == "Player")
        //                {
        //                    //skip the spawn
        //                    hasPlayer = true;
        //                    break;
        //                }
        //            }
        //            //is it the closest place I can put a bomb?
        //            if (hitInfo.distance < dist && hasPlayer == false)
        //            {
        //                dist = hitInfo.distance;
        //                place.Clear();
        //                place.Add(hitInfo.collider.gameObject);
        //            }
        //        }
        //    }
        //}
        //if (dist != placeDist + 1 && place.Count > 0)
        //{
        //    BombDropZone dropZone = place[0].GetComponent<BombDropZone>();
        //    GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
        //    bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
        //    bomb.GetComponent<Bomb>().Radius = bombRadius;
        //    dropZone.hasBomb = true;
        //    //placed bomb
        //    return true;
        //}
        ////return I didnt place
        //return false;















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
    public void PlaceBombIndicators()
    {
        foreach (GameObject ind in Indicators)
        {
            Destroy(ind);
        }
        PlaceDir directionOverwrite = PlaceDir.ObjectDirection;
        List<Vector3> IndicatorsLocation = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            float placeDist = 1f;
            //adjust height to match boxes
            Vector3 adjustment = transform.position;
            adjustment.y = 0.5f;
            Vector3 rayDest = adjustment;
            // set destination based on direction
            if (i == 0)
            {
                //raycast forward/+z
                rayDest.z += placeDist;
            }
            else if (i == 1)
            {
                //raycast backward/-z
                rayDest.z -= placeDist;
            }
            else if (i == 2)
            {
                //raycast left/-x
                rayDest.x -= placeDist;
            }
            else if (i == 3)
            {
                //raycast right/+z
                rayDest.x += placeDist;
            }
            RaycastHit hitInfo;
            if (Physics.Linecast(adjustment, rayDest, out hitInfo))
            {
                if (hitInfo.collider.tag == "BombDropZone")
                {
                    if (hitInfo.collider.GetComponent<BombDropZone>().hasBomb == false)
                    {
                        bool hasPlayer = false;
                        Collider[] inBombsPlace = Physics.OverlapSphere(hitInfo.collider.transform.position, 0.5f);
                        foreach (Collider col in inBombsPlace)
                        {
                            //player within bomb if spawned 
                            if (col.tag == "Player")
                            {
                                //skip the spawn
                                hasPlayer = true;
                                break;
                            }
                        }
                        if (hasPlayer == false)
                        {
                            IndicatorsLocation.Add(hitInfo.collider.gameObject.transform.position);
                            BombDropZone dropZone = hitInfo.collider.gameObject.GetComponent<BombDropZone>();
                            GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
                        }
                    }
                }
            }
        }
    }
}
