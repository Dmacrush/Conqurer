using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class EnemyAnimationHook : MonoBehaviour
    {

        public GameObject[] DamageCol;
        public GameObject EnemyParry;
        public bool PlayerParry = false;

        public InputHandler input;
       public StateManager States;
        Animator anim;

       public void Start()
        {
            anim = GetComponent<Animator>();
            CloseDamageCollider();
            CloseParryCollider();
            PlayerParry = false;

        }



        public void DamageColliderOpen()
        {

            OpenDamageCollider();
            input.OpenParryFlags();
        }

        public void DamgeColliderClose()
        {
           
            input.CloseParryFlags();
            CloseDamageCollider();
        }



        public void OpenDamageCollider()
        {

            for (int i = 0; i < DamageCol.Length; i++)
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



        public void OpenParryCollider()
        {

            EnemyParry.SetActive(true);
            PlayerParry = true;


        }
        public void CloseParryCollider()
        {

            PlayerParry = false;
            EnemyParry.SetActive(false);
        }


        /*  public void OnAnimatorMOve()
           {


               Vector3 delta = anim.deltaPosition;
               delta.y = 0;
               Vector3 v = (delta * 1) / Time.deltaTime;
               States.rigid.velocity = v;

               States.rigid.drag = 0;



           }*/



    }
}