using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA

{


    public class BossAnimationHook : MonoBehaviour
    {

        public GameObject[] DamageCol;
       public GameObject EnemyParry;
        public bool PlayerParry = false;

        public InputHandler input;
        public StateManager States;

        public void Start()
        {
            Close_DamageCollider();
            Close_ParryCollider();
            PlayerParry = false;

        }



        public void Damage_ColliderOpen()
        {
            Open_ParryCollider();
            Open_DamageCollider();
            input.OpenParryFlags();
        }

        public void Damge_ColliderClose()
        {

            input.CloseParryFlags();
            Close_ParryCollider();
            Close_DamageCollider();
        }



        public void Open_DamageCollider()
        {

            for (int i = 0; i < DamageCol.Length; i++)
            {

                DamageCol[i].SetActive(true);
            }
        }



        public void Close_DamageCollider()
        {

            for (int i = 0; i < DamageCol.Length; i++)
            {

                DamageCol[i].SetActive(false);
            }
        }



        public void Open_ParryCollider()
        {

            EnemyParry.SetActive(true);
            PlayerParry = true;


        }
        public void Close_ParryCollider()
        {

            PlayerParry = false;
            EnemyParry.SetActive(false);
        }


    

    }
}