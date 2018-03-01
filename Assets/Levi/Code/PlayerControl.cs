using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public BombPlacement placer;
    public float spawnTime;
    public float bombRadius;

    public PlayerIndex playerNumber;
    GamePadState state;
    GamePadState prevState;

    public float pickupTimer;
    public bool isInvincible = false;
    public ParticleSystem InvincibleParticles;


    private Rigidbody rb;
    private float startingSpawnTimer;
    private float startingPickUpTimer;
    public bool isAlive = true;
   
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
    //public float GetAxisFromController(string axisName)
    //{
    //    return Input.GetAxis(axisName + playerNumber);
    //}
    //public bool GetButtonFromController(string buttonName)
    //{
    //    return Input.GetButton(buttonName + playerNumber);
    //}

    private void move()
    {
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), .15f);
        }


        moveHorizontal = state.ThumbSticks.Left.X;
        moveVertical = state.ThumbSticks.Left.Y;


        movement = new Vector3(moveHorizontal, 0f, moveVertical);

        

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        

    }

    public void SetAlive(bool set = false)
    {
        isAlive = set;
    }


    //moved from fixed update due to conflicts with triggerzones
    private void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerNumber);

        move();

        //places the bomb
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    placer.PlaceBomb(1, bombRadius + .5f);
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
            gameObject.SetActive(false);
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
                if (bombRadius < 4)
                {
                    bombRadius += 1;
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "FireDown")
            {
                if (bombRadius > 1)
                {
                    bombRadius -= 1;
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "SlowPickUp")
            {
                if (speed > 3)
                {
                    speed -= 1;
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "SpeedPickUp")
            {
                if (speed != 6)
                {
                    speed += 1;
                }
                Destroy(other.gameObject);
            }
        }
    }

}
