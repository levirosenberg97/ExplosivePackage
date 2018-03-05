using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float Radius = 5;
    public float Damage = 1;
    public float fuseTime = 3;
    public GameObject Explosion;
    public GameObject SmallExplosion;
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
    struct ExplosionInfo
    {
        public List<Health> toBeDamaged;
        public List<Vector3> explosionEffects;
    }
    private ExplosionInfo ValidTargets(RaycastHit[] hit)
    {
        //holds items that are within blast radius in a direction that might need the explosion effect
        List<GameObject> potentialEffects = new List<GameObject>();
        //holds positions that will get the explosion effect spawned on them
        List<Vector3> explosions = new List<Vector3>();
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
                distToExplosion toAdd = new distToExplosion();
                toAdd.item = hit[i].collider.gameObject;
                toAdd.dist = Vector3.Distance(gameObject.transform.position, hit[i].collider.gameObject.transform.position); ;
                distanceToExplosion.Add(toAdd);
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
                //add as a place that might need the explision effect
                explosions.Add(distanceToExplosion[i].item.transform.position);
                //add wall to the to be damaged list
                retVal.Add(distanceToExplosion[i].item.GetComponent<Health>());
                //stop dealing damage in this direction
                i = distanceToExplosion.Count;
            }
            else if (distanceToExplosion[i].item.tag == "BombDropZone")
            {
                //add as a place that might need the explision effect
                explosions.Add(distanceToExplosion[i].item.transform.position);
                //check for players to damage inside this zone and within radius of explosion
                List<GameObject> interactors = distanceToExplosion[i].item.GetComponent<TriggerZone>().GetInteractors(TriggerState.All);
                foreach (GameObject interactor in interactors)
                {
                    if (interactor.tag == "Player" && Vector3.Distance(transform.position, interactor.transform.position) <= Radius)
                    {
                        Health hp = interactor.GetComponent<Health>();
                        if (retVal.Contains(hp) == false)
                        {
                            //add hp to list of to be damaged
                            retVal.Add(hp);
                        }
                    }
                    else if (interactor.tag == "Invincible" || interactor.tag == "FireUp" || interactor.tag == "FireDown" || interactor.tag == "SpeedPickUp" || interactor.tag == "SlowPickUp")
                    {
                        Health hp = interactor.GetComponent<Health>();
                        if (retVal.Contains(hp) == false)
                        {
                            retVal.Add(hp);
                        }
                    }
                }
            }
        }
        ExplosionInfo retValue;
        retValue.toBeDamaged = retVal;
        retValue.explosionEffects = explosions;
        return retValue;
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
        List<Vector3> explosionOrigins = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            ExplosionInfo info = ValidTargets(Physics.RaycastAll(gameObject.transform.position, (rayDir /** Radius*/), Radius));
            List<Health> toBeAdded = info.toBeDamaged;
                foreach (Health hp in toBeAdded)
                {
                    if (toBeDamaged.Contains(hp) == false)
                    {
                    if (Vector3.Distance(hp.gameObject.transform.position, transform.position) <= Radius)
                        toBeDamaged.Add(hp);
                    }
                }
                foreach(Vector3 explosionOrigin in info.explosionEffects)
            {
                explosionOrigins.Add(explosionOrigin);
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
        foreach (Vector3 position in explosionOrigins)
        {
            //spawn small explosion effect
            Instantiate(SmallExplosion, position, gameObject.transform.rotation);
        }
        //destroy bomb
        Destroy(gameObject);
    }
}
