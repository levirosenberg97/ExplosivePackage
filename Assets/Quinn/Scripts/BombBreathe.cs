using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBreathe : MonoBehaviour
{
    //public
    public Vector3 Amplitude = new Vector3(.25f, .25f, .25f);
    public Vector3 Frequency = new Vector3(1, 1, 1);
    public bool UsesFixedUpdate = false;
    //private
    private Vector3 SineWaveScaleOffset;

    // Use this for initialization
    void Start()
    {
        SineWaveScaleOffset = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (UsesFixedUpdate == false)
        {
            PreformScale();
        }
    }
    // FixedUpdate is called every 0.02 seconds
    private void FixedUpdate()
    {
        if (UsesFixedUpdate)
        {
            PreformScale();
        }
    }
    private void PreformScale()
    {
        Vector3 waveValues = new Vector3();
        waveValues.x += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency.x) * Amplitude.x;
        waveValues.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency.y) * Amplitude.y;
        waveValues.z += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency.z) * Amplitude.z;
        Vector3 tempScale = SineWaveScaleOffset + waveValues;
        transform.localScale = tempScale;
    }
}