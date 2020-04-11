using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{


    
    public class WeaponHook : MonoBehaviour
    {
        Parry P_parry;
        public GameObject[] DamageCol;



        public void Start()
        {
    
            P_parry = GetComponent<Parry>();
            ParryCollider pr = P_parry.GetComponent<ParryCollider>();
            CloseDamageCollider();

        }


        public void OpenDamageCollider()
        {
            for (int i =0; i <DamageCol.Length; i++)
            {

                DamageCol[i].SetActive(true);
            }
        }



        public void CloseDamageCollider()
        {
            for (int i = 0; i < DamageCol.Length; i++)
            {
                

                DamageCol[i].SetActive(false);
            }
        }


      

    }

}
