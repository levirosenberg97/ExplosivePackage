using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float Radius = 1;
    public float Damage = 1;
    public float fuseTime = 3;
    public GameObject Explosion;
    public GameObject DropZone;
    public List<string> Damages = new List<string>();
    //private 
    float timer = 0;
	// Use this for initialization
	void Start () {
	}
    void Update()
    {
        //add time to timer
        timer += Time.deltaTime;
        //explode the bomb if timer is more than fuse time
        if(timer > fuseTime)
        {
            //call explode
            Explode();
        }
    }
    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, Radius);
        List<Health> toBeDamaged = new List<Health>();
        int i = 0;
        //for each thing i hit with the sphere
        while (i < hitColliders.Length)
        {
            RaycastHit hit;
            //can I directly see the object?
            if (Physics.Raycast(transform.position, (hitColliders[i].transform.position - gameObject.transform.position), out hit, Radius))
            {
                //can I damage the object? and is it already on the list of items to be damaged?
                if (Damages.Contains(hit.collider.tag) && !toBeDamaged.Contains(hit.collider.GetComponent<Health>()))
                {
                    // damage object
                    toBeDamaged.Add(hit.collider.gameObject.GetComponent<Health>());
                }
            }
            i++;
        }
        //deal damage to all things hit
        foreach (Health hp in toBeDamaged)
        {
            hp.TakeDamage(Damage);
        }
        //spawn explosion effect
        Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
        //destroy bomb
        Destroy(gameObject);
    }
}
