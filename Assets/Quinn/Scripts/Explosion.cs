using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    private ParticleSystem particles;
    private float timer = 0;
	// Use this for initialization
	void Start () {
        particles = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (particles.main.duration / particles.main.simulationSpeed < timer)
        {
            particles.Stop();
            Destroy(gameObject);
        }
	}
}
