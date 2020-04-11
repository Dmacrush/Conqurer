using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA
{
    [CreateAssetMenu(menuName = "Varible/transfrom")]
    public class TransfromVarible : ScriptableObject
    {

        public Transform Value;
      
        public void Set(Transform V)
        {

            Value = V;

        }




    }


}

