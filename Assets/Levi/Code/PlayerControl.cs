using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    public GameObject bomb;

    public float spawnTime;

    private Rigidbody rb;

    private bool isAlive = true;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
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


        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");


        movement = new Vector3(moveHorizontal, 0f, moveVertical);

        

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        move();
        if (Input.GetKey(KeyCode.Space))
        {
            bombSpawner();
        }
        if (spawnTime < 3)
        {
            spawnTime += Time.deltaTime;
        }

    }

    void bombSpawner()
    {
        if (isAlive == true)
        {
            if (spawnTime >= 3)
            {
                Instantiate(bomb, transform.position, transform.rotation);
                spawnTime = 0;
            }

           
        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
