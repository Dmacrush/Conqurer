using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float RotationSpeed = 1;
    public Transform Target, Player;
    float MouseX, MouseY;


    public void Start()
    {


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



  public  void Update()
    {


        CameraControl();

    }


    void CameraControl()
    {

       
      
            MouseX += Input.GetAxis("Mouse X") * RotationSpeed;
            MouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;

            MouseY = Mathf.Clamp(MouseY, -35, 60);


            transform.LookAt(Target);

            

            Target.rotation = Quaternion.Euler(MouseY, MouseX, 0);
            Player.rotation = Quaternion.Euler(0, MouseX, 0);

        

       

    }

  

}
