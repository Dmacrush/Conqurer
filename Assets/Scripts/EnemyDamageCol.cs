using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class EnemyDamageCol : MonoBehaviour
    {

        public float PlayerDamage;
        private void OnTriggerEnter(Collider other)
        { 
            PlayerStats States = other.transform.GetComponent<PlayerStats>();

            if (States == null)
                return;
            States.DoDamage(PlayerDamage);



        }

    }
}