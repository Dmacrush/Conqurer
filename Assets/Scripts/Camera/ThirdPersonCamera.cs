using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA
{

    public class ThirdPersonCamera : MonoBehaviour
    {

        public TransfromVarible LockOnTranfrom;
        public float LookAngle;

        public float MouseSensitivity = 10;
        public Transform target;
        public float DistnaceFromTar = 2f;
        public Vector2 pitchMinMax = new Vector2(-40, 85);

        public float RotationSmoothTime = .12f;
        Vector3 RotationSmoothVelocity;
        Vector3 CurrentRotation;


        float Yaw;
        float pitch;







        private void Start()
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        }

        private void Update()
        {
          /*  if(LockOnTranfrom.Value != null)
            {
                Vector3 TargetDir = LockOnTranfrom.Value.position - transform.position;
                TargetDir.Normalize();
                TargetDir.y = 0;

                if(TargetDir == Vector3.zero)
                {
                    TargetDir = transform.forward;
                    Quaternion targetRot = Quaternion.LookRotation(TargetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);
                    LookAngle = transform.eulerAngles.y;
                    return;
                }


            }*/












            Yaw += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * MouseSensitivity;

            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            Vector3 targetRotation = new Vector3(pitch, Yaw);
            transform.eulerAngles = targetRotation;

            CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
            transform.eulerAngles = CurrentRotation;


            transform.position = target.position - transform.forward * DistnaceFromTar;








        }




    }

}

