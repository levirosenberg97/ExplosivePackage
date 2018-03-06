using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

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

    public Text powerUpText;
    public float fadeTimer;
    float startingFadeTime;

    private Rigidbody rb;
    private float startingSpawnTimer;
    private float startingPickUpTimer;
    public bool isAlive = true;
    //Quinn edit
    public float TextLife = 2;
    //private
    private GameObject InvincibleText;
    private GameObject SpeedUpText;
    private float speedUpTime = 0;
    private GameObject SpeedDownText;
    private float speedDownTime = 0;
    private GameObject FireUpText;
    private float fireUpTime = 0;
    private GameObject FireDownText;
    private float fireDownTime = 0;

    // Use this for initialization
    void Start ()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody>();
        startingPickUpTimer = pickupTimer;
        startingSpawnTimer = spawnTime;

        startingFadeTime = fadeTimer;
        foreach(Transform child in gameObject.transform)
        {
            if (child.name == "Invincible")
            {
                InvincibleText = child.gameObject;
            }
            else if (child.name == "Speed Up")
            {
                SpeedUpText = child.gameObject;
            }
            else if (child.name == "Speed Down")
            {
                SpeedDownText = child.gameObject;
            }
            else if (child.name == "Fire Up")
            {
                FireUpText = child.gameObject;
            }
            else if (child.name == "Fire Down")
            {
                FireDownText = child.gameObject;
            }
        }
	}

    float moveHorizontal;
    float moveVertical;
    Vector3 movement;

    private void move()
    {
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), .15f);
        }


        moveHorizontal = state.ThumbSticks.Right.X;
        moveVertical = state.ThumbSticks.Right.Y;


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
        if (powerUpText.color.a >= 1.0f)
        {
            fadeTimer -= Time.deltaTime;
        }

        if (fadeTimer <= 0.0f)
        {
            StartCoroutine(FadeTextToZeroAlpha(1f, powerUpText));
            fadeTimer = startingFadeTime;
        }

        prevState = state;
        state = GamePad.GetState(playerNumber);

        move();

        //places the bomb above

        if (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed)
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Forward))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Backward))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Left))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
        {
            if (isAlive == true)
            {
                if (spawnTime >= startingSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Right))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        //resets the bomb placement time
        if (spawnTime < startingSpawnTimer)
        {
            spawnTime += Time.deltaTime;
        }
        //manage text overhead
        if (SpeedDownText.activeInHierarchy)
        {
            speedDownTime += Time.deltaTime;
            if (speedDownTime >= TextLife)
            {
                SpeedDownText.SetActive(false);
                speedDownTime = 0;
            }
        }
        else if (SpeedUpText.activeInHierarchy)
        {
            speedUpTime += Time.deltaTime;
            if (speedUpTime >= TextLife)
            {
                SpeedUpText.SetActive(false);
                speedUpTime = 0;
            }
        }
        else if (FireUpText.activeInHierarchy)
        {
            fireUpTime += Time.deltaTime;
            if (fireUpTime >= TextLife)
            {
                FireUpText.SetActive(false);
                fireUpTime = 0;
            }
        }
        else if (FireDownText.activeInHierarchy)
        {
            fireDownTime += Time.deltaTime;
            if (fireDownTime >= TextLife)
            {
                FireDownText.SetActive(false);
                fireDownTime = 0;
            }
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
                InvincibleText.SetActive(false);
            }
        }
        //removes the player on death
        if (isAlive == false)
        {
            gameObject.SetActive(false);
        }

    }

   

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while(i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
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
                powerUpText.text = "Invincible";
                InvincibleText.SetActive(true);
                //fades text in
                StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));

                pickupTimer = startingPickUpTimer;
                Destroy(other.gameObject);
            }

            if (other.tag == "FireUp")
            {
                if (bombRadius < 4)
                {
                    bombRadius += 1;
                    FireUpText.SetActive(true);
                    fireUpTime = 0;
                    //fades text in
                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));

                    powerUpText.text = "Fire Up";
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "FireDown")
            {
                if (bombRadius > 1)
                {
                    bombRadius -= 1;
                    FireDownText.SetActive(true);
                    fireDownTime = 0;
                    //fades text in
                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));
             

                    powerUpText.text = "Fire Down";
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "SlowPickUp")
            {
                if (speed > 3)
                {
                    speed -= 1;
                    SpeedDownText.SetActive(true);
                    speedDownTime = 0;
                    //fades text in
                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));
                          

                    powerUpText.text = "Slowed";
                }
               
                Destroy(other.gameObject);
            }

            if (other.tag == "SpeedPickUp")
            {
                if (speed != 6)
                {
                    speed += 1;
                    SpeedUpText.SetActive(true);
                    speedUpTime = 0;
                    //fades text in
                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));
                  
                    powerUpText.text = "Sped Up";
                }
                Destroy(other.gameObject);
            }
        }
    }

}
