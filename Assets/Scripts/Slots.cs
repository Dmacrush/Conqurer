using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA

{
      

    public class Slots : MonoBehaviour
    {
        EquimetsManager equiments;
        public int i;




        private void Start()
        {


            equiments = GameObject.FindGameObjectWithTag("GameController").GetComponent<EquimetsManager>();
        }


        private void Update()
        {
            
            if(transform.childCount <=0)
            {
                equiments.IsFull[i] = false;

            }

        }
        public void DropItem()
        {
            foreach (Transform child in transform)
            {

                GameObject.Destroy(child.gameObject);


            }
        }



    }
}