using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSpinner : MonoBehaviour
{

    Rigidbody rb;
    TrailRenderer trail;
    public float rotationSpeed;


   public bool isActivated;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        
    }

   public void FixedUpdate()
    {
        

        if(isActivated)
        {

            transform.localEulerAngles +=  Vector3.forward* rotationSpeed * Time.deltaTime;
            trail.enabled = true;

        } else
        {
            trail.enabled = false;

        }

    }



    public void OnCollisionEnter(Collision collision)
    {

        isActivated = false;
        rb.isKinematic = true;

        


    }





}
