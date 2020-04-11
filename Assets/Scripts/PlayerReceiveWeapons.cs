using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA

{



    public class PlayerReceiveWeapons : MonoBehaviour
    {

        public GameObject Boss;
        private void Start()
        {

            Boss.SetActive(false);

        }
        public void OnTriggerEnter(Collider other)
        {

            PlayerStats player = other.transform.GetComponent<PlayerStats>();
           if(player == null)
            {
                return;
            } else
            {

                Boss.SetActive(true);
            }
           


        }




    }
}