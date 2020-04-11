using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{

    public class Parry : MonoBehaviour
    {

        public GameObject ParryCollider;





        public void Start()
        {

            CloseParryCollider();

        }
       

        public void OpenParryCollider()
        {
          
            ParryCollider.SetActive(true);
            


        }
        public void CloseParryCollider()
        {


            ParryCollider.SetActive(false);
        }


    }

}
