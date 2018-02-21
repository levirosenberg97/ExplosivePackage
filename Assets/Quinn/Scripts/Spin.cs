using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    //NOTE I COULDN'T THINK OF A BETTER NAME
    public float rotationRateX = 0;
    public float rotationRateY = 0;
    public float rotationRateZ = 0;
    [Header("Hover settings")]
    public float amplitude = 0.5f;
    public float frequency = 1f;
    //private
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
        transform.Rotate(rotationRateX, rotationRateY, rotationRateZ);
    }
}