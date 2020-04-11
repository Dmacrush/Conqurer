using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{

    public class BossParry : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats States = other.transform.GetComponent<PlayerStats>();

            if (States == null)
                return;

            States.GettingParried();



        }

    }
}