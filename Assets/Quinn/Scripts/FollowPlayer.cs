using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public float y = 1;
    public GameObject Leader;
    void Start()
    {
        Vector3 pos = Leader.transform.position;
        pos.y = y;
        transform.SetPositionAndRotation(pos, transform.rotation);
    }
    void Update()
    {
        Vector3 pos = Leader.transform.position;
        pos.y = y;
        transform.SetPositionAndRotation(pos, transform.rotation);
    }
}
