using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectionEvent : MonoBehaviour
{


    public bool deflection;



    private void Update()
    {
        

        if(deflection == true)
        {


            if(Input.GetKey(KeyCode.Mouse1))
            {
                Debug.Log("Deflected");
            }
        }


    }







    public void DeflectionEnemy()
    {


        deflection = true;

        
    }



}
