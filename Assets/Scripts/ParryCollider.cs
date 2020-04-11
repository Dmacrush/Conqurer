using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA
{
    public class ParryCollider : MonoBehaviour
    {

        StateManager states;
        EnemyStates Estates;
        BossStates Bstates;
        public float MaxTimer;
        float Timer;

        public void InitPlayer(StateManager st)
        {

            states = st;
        }


        public void Initenemy(EnemyStates Est)
        {

            Estates = Est;

        }

        public void InitBoss(BossStates Bst)
        {

            Bstates = Bst;
        }

        public void Update()
        { 
        
            if(states )
            {

                Timer += states.Delta;
                if(Timer > MaxTimer)
                {
                    Timer = 0;
                    gameObject.SetActive(false);
                }
            }

            if (Estates)
            {
                Timer += Estates.delta;
                if (Timer > MaxTimer)
                {
                    Timer = 0;
                    gameObject.SetActive(false);
                }

            }


            if (Bstates)
            {
                Timer += Bstates.delta;
                if (Timer > MaxTimer)
                {
                    Timer = 0;
                    gameObject.SetActive(false);
                }

            }

        }

        public void OnTriggerEnter(Collider other)
        {
       

            if(states)
                { 
            EnemyStates e_st = other.transform.GetComponentInParent<EnemyStates>();

            if (e_st == null)
            {
                e_st.CheckForParry(transform.root,states);
               
            }

            }


            if (Bstates)
            {


                BossStates st = other.transform.GetComponent<BossStates>();
                if (st == null)
                {
                    Debug.Log("Parry");
                    st.CheckForParry(transform.root, states);
                }


            }

        }




    }
}
