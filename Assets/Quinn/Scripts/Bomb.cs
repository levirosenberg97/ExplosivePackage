using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float Radius = 5;
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
        drawShootLines();
       

        if(timer > fuseTime)
        {
            //call explode
            Explode();
        }
    }
    struct distToExplosion
    {
        public GameObject item;
        public float dist;
    };
    private List<Health> ValidTargets(RaycastHit[] hit)
    {
        
        //list of health components that will take damage from this exposion
        List<Health> retVal = new List<Health>();
        //list of gameobjects and their distance to the center of the explosion
        List<distToExplosion> distanceToExplosion = new List<distToExplosion>();
        //collect items the bomb should be looking for (walls it can damage, players and walls it can't damage)
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.tag == "UnbreakableWall")
            {
                distToExplosion toAdd = new distToExplosion();
                toAdd.item = hit[i].collider.gameObject;
                toAdd.dist = Vector3.Distance(gameObject.transform.position, hit[i].collider.gameObject.transform.position);
                distanceToExplosion.Add(toAdd);
            }
            else if (hit[i].collider.tag == "BreakableWall")
            {
                distToExplosion toAdd = new distToExplosion();
                toAdd.item = hit[i].collider.gameObject;
                toAdd.dist = Vector3.Distance(gameObject.transform.position, hit[i].collider.gameObject.transform.position);
                distanceToExplosion.Add(toAdd);
            }
            else if (hit[i].collider.tag == "BombDropZone")
            {
                TriggerZone zone = hit[i].collider.gameObject.GetComponent<TriggerZone>();
                float dist = Vector3.Distance(gameObject.transform.position, hit[i].collider.gameObject.transform.position);
                int interactors = zone.GetInteractors(TriggerState.All).Count;
                foreach (GameObject interactor in zone.GetInteractors(TriggerState.All))
                {
                    distToExplosion toAdd = new distToExplosion();
                    toAdd.item = interactor;
                    toAdd.dist = dist;
                    distanceToExplosion.Add(toAdd);
                }
            }
        }
        //orders by distance to explosion
        bool inWrongOrder = true;
        while(inWrongOrder)
        {
            if(distanceToExplosion.Count > 0)
            {
                float previous = distanceToExplosion[0].dist;
                bool correct = true;
                for (int i = 0; i < distanceToExplosion.Count; ++i)
                {
                    if (distanceToExplosion[i].dist < previous)
                    {
                        //wrong order
                        correct = false;
                        //flip the previous value with this one so the smaller of the two is in front
                        distToExplosion temp = distanceToExplosion[i];
                        distanceToExplosion[i] = distanceToExplosion[i - 1];
                        distanceToExplosion[i - 1] = temp;
                    }
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
        //determines what takes damage/where blast should stop
        for (int i = 0; i < distanceToExplosion.Count; i++)
        {
            if (distanceToExplosion[i].item.tag == "UnbreakableWall")
            {
                //stop dealing damage in this direction
                i = distanceToExplosion.Count;
            }
            else if (distanceToExplosion[i].item.tag == "BreakableWall")
            {
                //add wall to the to be damaged list
                retVal.Add(distanceToExplosion[i].item.GetComponent<Health>());
                //stop dealing damage in this direction
                i = distanceToExplosion.Count;
            }
            else if (distanceToExplosion[i].item.tag == "Player")
            {
                Health hp = distanceToExplosion[i].item.GetComponent<Health>();
                //if the list does not already contain this player (stops double damage)
                if (retVal.Contains(hp) == false)
                {
                    retVal.Add(hp);
                }
            }
        }
        return retVal;
    }


    void drawShootLines()
    {
        Vector3 rayDir = transform.forward;

        for (int i = 0; i < 4; i++)
        {
            //forward check
            //if (i == 0)
            //{
            Debug.DrawLine(transform.position, transform.position + (rayDir * Radius), Color.red);


            rayDir = Quaternion.AngleAxis(90, transform.up) * rayDir;
        }
    }
    public void Explode()
    {
        Vector3 rayDir = transform.forward;
        List<Health> toBeDamaged = new List<Health>();
        for (int i = 0; i < 4; i++)
        {
            List <Health> toBeAdded = ValidTargets(Physics.RaycastAll(gameObject.transform.position, (rayDir /** Radius*/), Radius));
                foreach (Health hp in toBeAdded)
                {
                    if (toBeDamaged.Contains(hp) == false)
                    {
                        toBeDamaged.Add(hp);
                    }
                }
            rayDir = Quaternion.AngleAxis(90, transform.up) * rayDir;
        }
        //deal damage
        foreach (Health hp in toBeDamaged)
        {
            hp.TakeDamage(Damage);
        }
        //tell the triggerzone that it no longer contains a bomb
        DropZone.GetComponent<BombDropZone>().hasBomb = false;
        //spawn explosion effect
        Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
        //destroy bomb
        Destroy(gameObject);
    }
}
