using System;
using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItems : MonoBehaviour
{
     
    public GameObject Items;
    public bool Ishere;



    private void Start()
    {
        Ishere = false;
        Items.SetActive(false);
           
    }


    private void Update()
    {

        if (Ishere == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<ScoreManager>().IncreaseScore();
                Items.SetActive(false);
                Destroy(this.gameObject);
            } 
                

        }

         


    }
   

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Items.SetActive(true);
            Ishere = true;


        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Items.SetActive(false);
            Ishere = false;

        }

    }

}