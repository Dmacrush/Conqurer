using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationplayer : MonoBehaviour
{
    Animator anim;



    private void Start()
    {
        anim = GetComponent<Animator>();

    }
    void Update()
    {
        
       if(Input.GetKey(KeyCode.Alpha1))
        {

            anim.Play("ComingDown");
            transform.Rotate(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {

            anim.Play("New Animation");
        }


    }
}
