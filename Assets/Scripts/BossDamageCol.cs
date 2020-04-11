using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{




    public class BossDamageCol : MonoBehaviour
    {


        public float BossDamageRate;

        private void OnTriggerEnter(Collider other)
        {
            BossStates States = other.transform.GetComponent<BossStates>();

            if (States == null)
                return;

            States.DoDamage(BossDamageRate);



        }


    }
}