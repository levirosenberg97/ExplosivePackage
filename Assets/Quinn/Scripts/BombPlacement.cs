using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour {
    public GameObject BombObject;
    public GameObject Indicator;
    public bool EnableIndicators = true;
    private List<Vector3> PreviousIndicators = new List<Vector3>();
    private List<GameObject> Indicators = new List<GameObject>();
    // Use this for initialization
    void Start() {
        List<Vector3> newIndicators = PlaceBombIndicators();
        foreach (Vector3 pos in newIndicators)
        {
            Indicators.Add(Instantiate(Indicator, pos, gameObject.transform.rotation));
        }
        //
        //Debug.Log(Indicators.Count);
    }

    // Update is called once per frame
    void Update() {
        if (EnableIndicators)
        {
            //Debug.Log(Indicators.Count);
            List<Vector3> newIndicators = PlaceBombIndicators();
            List<Vector3> IndicatorsPos = new List<Vector3>();
            foreach (GameObject ind in Indicators)
            {
                IndicatorsPos.Add(ind.gameObject.transform.position);
            }
            if (!isListEqual(newIndicators , IndicatorsPos))
            {
                if (Indicators.Count > 0 )
                {
                    while(Indicators.Count > 0)
                    {
                        GameObject ind = Indicators[0];
                        Indicators.Remove(ind);
                        Destroy(ind);
                    }
                }
                foreach(Vector3 pos in newIndicators)
                {
                    Indicators.Add(Instantiate(Indicator, pos, gameObject.transform.rotation));
                }
            }
            PreviousIndicators = newIndicators;
            Debug.Log(Indicators.Count);
        }
    }

    bool isListEqual(List<Vector3> listA, List<Vector3> listB)
    {
        int check = 0;
        if (listA.Count != listB.Count)
        {
            return false;
        }
        else
        {
           
            for (int i = 0; i < listA.Count;i++)
            {
                if(listA[i] == listB[i])
                {
                    check++;
                }
            }
            
        }
        return check == listA.Count;
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
    }
    struct PlaceInfo
    {
        public BombDropZone Zone;
        public float Dist;
    }
    public List<Vector3> PlaceBombIndicators()
    {
        List<Vector3> IndicatorsLocation = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            float placeDist = 1f;
            //adjust height to match boxes
            Vector3 adjustment = transform.position;
            adjustment.y = 0.5f;
            Collider[] DropZones = Physics.OverlapSphere(adjustment, placeDist/2);
            List<PlaceInfo> info = new List<PlaceInfo>();
            foreach (Collider col in DropZones)
            {
                BombDropZone zone = col.GetComponent<BombDropZone>();
                if (zone != null)
                {
                    if (zone.GetComponent<TriggerZone>().Contains(gameObject))
                    {
                        PlaceInfo pinfo;
                        pinfo.Zone = zone;
                        pinfo.Dist = Vector3.Distance(zone.transform.position, adjustment);
                        info.Add(pinfo);
                    }
                }
            }
            bool inWrongOrder = true;
            while (inWrongOrder)
            {
                if (info.Count > 0)
                {
                    float previous = info[0].Dist;
                    bool correct = true;
                    for (int j = 0; j < info.Count; ++j)
                    {
                        if (info[j].Dist < previous)
                        {
                            //wrong order
                            correct = false;
                            //flip the previous value with this one so the smaller of the two is in front
                            PlaceInfo temp = info[j];
                            info[j] = info[j - 1];
                            info[j - 1] = temp;
                        }
                        previous = info[j].Dist;
                    }
                    //was it in the correct order
                    if (correct)
                    {
                        inWrongOrder = false;
                    }
                }
                else
                {
                    //nothing to sort
                    inWrongOrder = false;
                }

            }
            if (info.Count >0)
            {
                adjustment = info[0].Zone.transform.position;
            }
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
                            //BombDropZone dropZone = hitInfo.collider.gameObject.GetComponent<BombDropZone>();
                            //GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
                        }
                    }
                }
            }

        }
        return IndicatorsLocation;
    }
    void OnDisable()
    {
        Debug.Log(Indicators.Count);
        if (Indicators.Count > 0)
        {
            while (Indicators.Count > 0)
            {
                GameObject ind = Indicators[0];
                Indicators.Remove(ind);
                Destroy(ind);
            }
        }
    }
}
