using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{

    public class DamageCollider : MonoBehaviour
    {


        public float EnemyDamage;
        private void OnTriggerEnter(Collider other)
        {


            EnemyStates eStates = other.transform.GetComponentInChildren<EnemyStates>();
            if (eStates == null)
                return;
            else
            eStates.DoDamage(EnemyDamage);



         
        }



    }

}




