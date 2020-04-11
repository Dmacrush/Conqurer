using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{


    public float speed = 10f;
    public void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Space))
        {


            GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }

    }



}
