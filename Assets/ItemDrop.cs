using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    public GameObject items;
    //public List<Transform> items = new List<Transform>();
    float dropRate = .25f;
    Health wallHP;

    private void Start()
    {
        wallHP = GetComponent<Health>();
    }


    private void Update()
    {
        if (wallHP.CurrentHP <= 0)
        {
            if (Random.Range(0, 1) <= dropRate)
            {
                var pickUpDrop = Instantiate(items, gameObject.transform.position, Quaternion.identity);
            }
           
        
        }
    }
   
}
