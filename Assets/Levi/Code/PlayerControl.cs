using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public AudioSource powerUp;
    public AudioSource powerDown;
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
    public float currentSpawnTimer;
    private float startingPickUpTimer;
    public bool isAlive = true;
    public GameObject PowerTextManagerObj;
    private TextManager PowerTextManager;

    public float vibrationCounter = 1;

    // Use this for initialization
    void Start ()
    {

        isAlive = true;

        rb = GetComponent<Rigidbody>();
        startingPickUpTimer = pickupTimer;
        startingSpawnTimer = spawnTime;

        startingFadeTime = fadeTimer;
        foreach(Transform child in transform)
        {
            if (child.name == "TextManager")
            {
                PowerTextManager = child.GetComponent<TextManager>();
            }
        }
        currentSpawnTimer = spawnTime;
	}

    float moveHorizontal;
    float moveVertical;
    Vector3 movement;

    private void move()
    {
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 9 * Time.deltaTime);
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
        if (isAlive == false)
        {
            vibrationCounter -= Time.deltaTime;
            if (vibrationCounter <= 0)
            {
                GamePad.SetVibration(playerNumber, 0, 0);
            }
        }

        if (powerUpText.color.a >= .9f)
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

        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed && Time.timeScale != 0)
        {
            if (isAlive == true)
            {
                if (spawnTime >= currentSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Forward))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && Time.timeScale != 0)
        {
            
            if (isAlive == true)
            {
                if (spawnTime >= currentSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Backward))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed && Time.timeScale != 0)
        {
            if (isAlive == true)
            {
                if (spawnTime >= currentSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Left))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && Time.timeScale != 0)
        {
            if (isAlive == true)
            {
                if (spawnTime >= currentSpawnTimer)
                {
                    if (placer.PlaceBomb(1, bombRadius + .5f, BombPlacement.PlaceDir.Right))
                    {
                        spawnTime = 0;
                    }
                }
            }
        }

        //resets the bomb placement time
        if (spawnTime < currentSpawnTimer)
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
            GamePad.SetVibration(playerNumber, 0, 0);
           
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
            if (other.tag == "BombUp")
            {
                if(spawnTime > 1f)
                {
                    powerUp.Play();

                    spawnTime -= .5f;
                    if (spawnTime <= 1f)
                    {
                        spawnTime = 1;
                    }
                    currentSpawnTimer = spawnTime;
                    powerUpText.text = "Bomb Up";

                    PowerTextManager.SpawnText(TextManager.TextString.BombUp);

                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));
                   
                }
                Destroy(other.gameObject);
            }

            if (other.tag == "BombDown")
            {
                if (spawnTime < startingSpawnTimer)
                {
                    powerDown.Play();
                    spawnTime += .5f;
                    if (spawnTime >= startingSpawnTimer)
                    {
                        spawnTime = startingSpawnTimer;
                    }
                    currentSpawnTimer = spawnTime;
                    powerUpText.text = "Bomb Down";

                    PowerTextManager.SpawnText(TextManager.TextString.BombDown);

                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));

                }
                Destroy(other.gameObject);
            }

            if ( other.tag == "Invincible")
            {
                powerUp.Play();

                isInvincible = true;
                InvincibleParticles.gameObject.SetActive(true);
                powerUpText.text = "Invincible";
                PowerTextManager.SpawnText(TextManager.TextString.Invincible);
                //fades text in
                StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));

                pickupTimer = startingPickUpTimer;
                Destroy(other.gameObject);
            }

            if (other.tag == "FireUp")
            {
                if (bombRadius < 4)
                {
                    powerUp.Play();

                    bombRadius += 1;
                    PowerTextManager.SpawnText(TextManager.TextString.FireUp);
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
                    powerDown.Play();

                    bombRadius -= 1;
                    PowerTextManager.SpawnText(TextManager.TextString.FireDown);
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
                    powerDown.Play();

                    speed -= 1;
                    PowerTextManager.SpawnText(TextManager.TextString.SpeedDown);
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
                    powerUp.Play();
                    speed += 1;
                    PowerTextManager.SpawnText(TextManager.TextString.SpeedUp);
                    //fades text in
                    StartCoroutine(FadeTextToFullAlpha(1f, powerUpText));
                  
                    powerUpText.text = "Sped Up";
                }
                Destroy(other.gameObject);
            }
        }
    }

}
