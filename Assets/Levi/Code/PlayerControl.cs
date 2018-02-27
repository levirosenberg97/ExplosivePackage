using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public BombPlacement placer;
    public float spawnTime;
    public int playerNumber;
    public float pickupTimer;
    public bool isInvincible = false;
    public ParticleSystem InvincibleParticles;


    private Rigidbody rb;
    private float startingSpawnTimer;
    private float startingPickUpTimer;
    private bool isAlive = true;
   
    // Use this for initialization
    void Start ()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody>();
        startingPickUpTimer = pickupTimer;
        startingSpawnTimer = spawnTime;
	}

    float moveHorizontal;
    float moveVertical;
    Vector3 movement;

    //takes an axis name and will add the appropreate player number to the end then return the input from that axis
    public float GetAxisFromController(string axisName)
    {
        return Input.GetAxis(axisName + playerNumber);
    }
    public bool GetButtonFromController(string buttonName)
    {
        return Input.GetButton(buttonName + playerNumber);
    }

    private void move()
    {
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), .15f);
        }


        moveHorizontal = GetAxisFromController("Horizontal");
        moveVertical = -GetAxisFromController("Vertical");


        movement = new Vector3(moveHorizontal, 0f, moveVertical);

        

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        

    }
    //moved from fixed update due to conflicts with triggerzones
    private void Update()
    {
        //places the bomb
        if (GetButtonFromController("Bomb"))
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    placer.PlaceBomb();
                    spawnTime = 0;
                }
            }
        }
        //resets the bomb placement time
        if (spawnTime < startingSpawnTimer)
        {
            spawnTime += Time.deltaTime;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        move();

        //makes the invincible powerup work
        if (isInvincible == true)
        {
            pickupTimer -= Time.deltaTime;
            isAlive = true;
            if (pickupTimer <= 0)
            {
                isInvincible = false;
                pickupTimer = startingPickUpTimer;
                InvincibleParticles.gameObject.SetActive(false);
            }
        }
        //removes the player on death
        if (isAlive == false)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlive == true)
        {
            if ( other.tag == "Invincible")
            {
                isInvincible = true;
                InvincibleParticles.gameObject.SetActive(true);
                Destroy(other.gameObject);
            }

            if (other.tag == "FireUp")
            {
                Destroy(other.gameObject);
            }

            if (other.tag == "FireDown")
            {
                Destroy(other.gameObject);
            }

            if (other.tag == "SlowPickUp")
            {
                if (speed > 3)
                {
                    speed -= 3;
                    Destroy(other.gameObject);
                }
            }

            if (other.tag == "SpeedPickUp")
            {
                if (speed != 9)
                {
                    speed += 3;
                    Destroy(other.gameObject);
                }
            }
        }
    }

}
