using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour {
    public GameObject BombObject;
    public GameObject Indicator;
    public bool EnableIndicators = true;
    private List<GameObject> Indicators = new List<GameObject>();
    // Use this for initialization
    void Start() {
        Transform pos = PlaceBombIndicators().transform;
        Indicators.Add(Instantiate(Indicator, pos.transform.position, pos.transform.rotation));
    }

    // Update is called once per frame
    void Update() {
        if (EnableIndicators)
        {
            if (Indicators.Count > 0)
            {
                while (Indicators.Count > 0)
                {
                    GameObject ind = Indicators[0];
                    Indicators.Remove(ind);
                    Destroy(ind);
                }
            }
            if (PlaceBombIndicators() != null)
            {
                Transform place = PlaceBombIndicators().transform;
                Indicators.Add(Instantiate(Indicator, place.transform.position, place.transform.rotation));
            }
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
        //found zone, spawn bomb
        if (PlaceBombIndicators() != null)
        {
            BombDropZone dropZone = PlaceBombIndicators();
            GameObject bomb = Instantiate(BombObject, dropZone.transform.position, dropZone.transform.rotation);
            bomb.GetComponent<Bomb>().DropZone = dropZone.gameObject;
            bomb.GetComponent<Bomb>().Radius = bombRadius;
            dropZone.hasBomb = true;
            Collider[] inBombsPlace = Physics.OverlapSphere(dropZone.gameObject.transform.position, 0.5f);
            foreach (Collider col in inBombsPlace)
            {
                //player within bomb if spawned (ignore collision)
                if (col.tag == "Player")
                {
                    bomb.GetComponent<Bomb>().IgnoredPlayers.Add(col.gameObject);
                    Physics.IgnoreCollision(col, bomb.GetComponent<Collider>(), true);
                }
            }
            return true;
        }
        //couldn't find zone, didn't spawn bomb
        else
        {
            return false;
        }
    }
    struct PlaceInfo
    {
        public BombDropZone Zone;
        public float Dist;
    }
    public BombDropZone PlaceBombIndicators()
    {
        //adjust height to match boxes
        Vector3 adjustment = transform.position;
        adjustment.y = 0.5f;
        Collider[] hit = Physics.OverlapSphere(adjustment, 0.5f);
        List<PlaceInfo> zones = new List<PlaceInfo>();
        if (hit.Length > 0)
        {
            foreach (Collider obj in hit)
            {
                BombDropZone attempt = obj.GetComponent<BombDropZone>();
                if (attempt != null && attempt.hasBomb == false)
                {
                    PlaceInfo val;
                    val.Zone = attempt;
                    val.Dist = Vector3.Distance(obj.gameObject.transform.position, adjustment);
                    zones.Add(val);
                }
            }
        }
        if (zones.Count > 0)
        {
            bool inWrongOrder = true;
            while (inWrongOrder)
            {
                if (zones.Count > 0)
                {
                    float previous = zones[0].Dist;
                    bool correct = true;
                    for (int j = 0; j < zones.Count; ++j)
                    {
                        if (zones[j].Dist < previous)
                        {
                            //wrong order
                            correct = false;
                            //flip the previous value with this one so the smaller of the two is in front
                            PlaceInfo temp = zones[j];
                            zones[j] = zones[j - 1];
                            zones[j - 1] = temp;
                        }
                        previous = zones[j].Dist;
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
        }
        //found zone
        if (zones.Count > 0)
        {
            return zones[0].Zone;
        }
        //didn't find zone
        else
        {
            return null;
        }
    }
    void OnDisable()
    {
        //Debug.Log(Indicators.Count);
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
