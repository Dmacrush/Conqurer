using System;
using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool LockOn;

    public float MouseSpeed = 2;
    public float FollowSpeed = 9;
    public float DefZ;
    public float zSpeed = 19f;
    float CurZ;

    StateManager States;


    public Transform Target;

    public EnemyTarget LockOnTarget;

    //public BossTarget BLockOnTarget;
    public Transform LockonTransform;
    float TrunSmoothing = .1f;
    public float minAngle = -35;
    public float maxAngle = 35;
    public float lookAngle;
    public float TiltAngle;
    float SmoothX;
    float SmoothY;
    float SmoothVelocityX;
    float SmoothVelocityY;
    bool UsedAxis;

    bool ChangeTargetLeft;
    bool ChangeTargetRight;

    private void Start()
    {
        //  Cursor.lockState = CursorLockMode.Locked;
        //  Cursor.visible = false;
    }


    public Transform Pivot;
    public Transform CamTurns;

    public void Tick(float d)
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");


        ChangeTargetLeft = Input.GetKeyUp(KeyCode.V);
        ChangeTargetRight = Input.GetKeyUp(KeyCode.C);
        if (LockOnTarget != null)
        {
            if (LockonTransform == null)
            {
                LockonTransform = LockOnTarget.GetTarget();
                States.EnemyLockOnTransform = LockonTransform;
            }

            if (Mathf.Abs(h) > 0.6f)
            {
                if (!UsedAxis)
                {
                    LockonTransform = LockOnTarget.GetTarget((h > 0));
                    States.EnemyLockOnTransform = LockonTransform;
                    UsedAxis = true;
                }
            }


            if (ChangeTargetLeft || ChangeTargetRight)
            {
                LockonTransform = LockOnTarget.GetTarget(ChangeTargetLeft);
                States.EnemyLockOnTransform = LockonTransform;
            }
        }

        /*   if (BLockOnTarget != null)
               {
   
   
                   if (LockonTransform == null)
                   {
                       LockonTransform = BLockOnTarget.GetTraget();
                       States.EnemyLockOnTransform = LockonTransform;
   
                   }
   
   
                   if (Mathf.Abs(h) < 0.06f)
                   {
                       if (!UsedAxis)
                       {
   
                           LockonTransform = BLockOnTarget.GetTraget((h  > 0));
                           States.EnemyLockOnTransform = LockonTransform;
                           UsedAxis = true;
   
                       }
   
   
                   }
   
   
   
   
                   if (ChangeTargetLeft || ChangeTargetRight)
                   {
   
                       LockonTransform = BLockOnTarget.GetTraget(ChangeTargetLeft);
                       States.EnemyLockOnTransform = LockonTransform;
                   }
   
   
               }*/

        if (UsedAxis)
        {
            if (Mathf.Abs(h) < 0.6f)
            {
                UsedAxis = false;
            }
        }

        float targetSpeed = MouseSpeed;
        HandleRotation(d, v, h, targetSpeed);

        FollowPlayer(d);
        HandlePivotPos();
    }


    public void FollowPlayer(float d)
    {
        float Speed = d * FollowSpeed;
        Vector3 targetpos = Vector3.Lerp(transform.position, Target.position, Speed);
        transform.position = targetpos;
    }

    public void Init(StateManager st)
    {
        States = st;
        Target = st.transform;
        CamTurns = Camera.main.transform;
        Pivot = CamTurns.parent;
        CurZ = DefZ;
    }

    public void HandleRotation(float d, float h, float v, float targetSpeed)
    {
        if (TrunSmoothing > 0)
        {
            SmoothX = Mathf.SmoothDamp(SmoothX, v, ref SmoothVelocityX, TrunSmoothing);
            SmoothY = Mathf.SmoothDamp(SmoothY, h, ref SmoothVelocityY, TrunSmoothing);
        }
        else
        {
            SmoothX = h;
            SmoothY = v;
        }

        TiltAngle -= SmoothY * targetSpeed;
        TiltAngle = Mathf.Clamp(TiltAngle, minAngle, maxAngle);
        Pivot.localRotation = Quaternion.Euler(TiltAngle, 0, 0);

        if (LockOn && LockOnTarget != null)
        {
            Vector3 Direction = LockonTransform.position - transform.position;
            Direction.Normalize();
            //Direction.y = 0;

            if (Direction == Vector3.zero)
                Direction = transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(Direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);

            lookAngle += transform.eulerAngles.y;


            return;
        }


        lookAngle += SmoothX * targetSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);


        /*    if (LockOn && BLockOnTarget != null)
                {
                    Vector3 Direction = LockonTransform.position - transform.position;
                    Direction.Normalize();
                    //Direction.y = 0;
    
                    if (Direction == Vector3.zero)
                        Direction = transform.forward;
                    Quaternion targetRot = Quaternion.LookRotation(Direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);
    
                    lookAngle += transform.eulerAngles.y;
    
    
                    return;
    
    
                }
                */


        /*   if (LockOnTransform.Value != null)
               {

                   Vector3 targetdir = LockOnTransform.Value.position - transform.position;
                   targetdir.Normalize();
                   targetdir.y = 0;


                   if (targetdir == Vector3.zero)
                       targetdir = transform.forward;
                   Quaternion tr = Quaternion.LookRotation(targetdir);
                   transform.rotation = Quaternion.Slerp(transform.rotation,tr,d*9);
                   lookAngle = transform.eulerAngles.y;


                   Vector3 tiltdire = LockOnTransform.Value.position - Pivot.position;
                   tiltdire.Normalize();
                   if (tiltdire == Vector3.zero)
                       tiltdire = Pivot.forward;
                   Quaternion tiltRot = Quaternion.LookRotation(tiltdire);
                   Pivot.rotation = Quaternion.Slerp(Pivot.rotation, tiltRot, d * 9);
                   TiltAngle = Pivot.eulerAngles.x;

                       return;


               }*/
    }

    public void HandlePivotPos()
    {
        float targetZ = DefZ;

        CameraColission(DefZ, ref targetZ);

        CurZ = Mathf.Lerp(CurZ, targetZ, States.Delta * zSpeed);

        Vector3 tp = Vector3.zero;
        tp.z = CurZ;

        CamTurns.localPosition = tp;
    }

    public void CameraColission(float TargetZ, ref float Actualz)
    {
        float step = Mathf.Abs(TargetZ);
        int StepCount = 2;
        float StepIncrement = step / StepCount;


        RaycastHit hit;

        Vector3 origin = Pivot.position;
        Vector3 _Direction = -Pivot.forward;

        if (Physics.Raycast(origin, _Direction, out hit, step, States.IgnoreLayers))
        {
            float disnace = Vector3.Distance(hit.point, origin);
            Actualz = -(disnace / 2);
        }
        else
        {
            for (int s = 0; s < StepCount + 1; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 dir = Vector3.zero;
                    Vector3 SecondOrigin = origin + (_Direction * s) * StepIncrement;

                    switch (i)
                    {
                        case 0:
                            dir = CamTurns.right;
                            break;
                        case 1:
                            dir = -CamTurns.right;
                            break;

                        case 2:
                            dir = CamTurns.up;

                            break;

                        case 3:
                            dir = -CamTurns.up;
                            break;
                    }

                    if (Physics.Raycast(SecondOrigin, dir, out hit, 0.2f, States.IgnoreLayers))
                    {
                        float distnace = Vector3.Distance(SecondOrigin, origin);
                        Actualz = -(distnace / 2);
                        if (Actualz < 0.2f)
                            Actualz = 0;
                        return;
                    }
                }
            }
        }
    }

    public static CameraManager Singleton;

    public void Awake()
    {
        Singleton = this;
    }
}