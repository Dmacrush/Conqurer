using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace SA
{
    public class StateManager : MonoBehaviour
    {
        public bool IsnotMoving = false;
        public bool IsBackStabHappining = true;
        public string[] Attacks;
        public float DistoGr = 0.3f;
        public float Vertical;
        public float Horizontal;
        public Vector3 MoveDir;
        public float MoveAount;
        public float MoveSpeed = 2;
        public float Runspeed = 6;
        public float RotatioonSpeed = 5f;
        public bool run;
        public bool OnGround;
        public float DistanceToGround = 0.5f;
        public LayerMask IgnoreLayers;
        public bool RollInput;
        public float ParryOffset = 1.4f;
        public float BackStabOffset = 1.4f;

        public float JumpSpeed;


        [Header("Inputs")] public bool rt, rb, lt, lb;
        public bool InAction;
        public bool CanMove;
        float _actionDelay;
        public bool CanBeParried;
        public bool ParryIsOn;
        public float parryTimer;

        CapsuleCollider col;

        public EnemyTarget EnemyTarget;

        // public BossTarget BossTarget;
        public Transform EnemyLockOnTransform;


        public GameObject ActiveMOdel;
        public Animator Anim;
        public Rigidbody rigid;
        public float Delta;
        public bool LockOn;

        public bool RightMOuse;
        public EnemyAnimationHook E_Hook;


        public void Init()
        {
            col = GetComponent<CapsuleCollider>();
            SetUpAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            gameObject.layer = 8;
            IgnoreLayers = ~(1 << 9);
            PreviousGround = true;
        }


        public void SetUpAnimator()
        {
            if (ActiveMOdel == null)
            {
                Anim.GetComponentInChildren<Animator>();
                if (Anim == null)
                {
                    Debug.Log("No model found");
                }
                else
                {
                    ActiveMOdel = Anim.gameObject;
                }
            }


            if (Anim == null)
            {
                Anim = ActiveMOdel.GetComponent<Animator>();
                Anim.applyRootMotion = false;
            }
        }

        public void Update()
        {
            if (PreviousGround == true)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }


            if (IsnotMoving == true)
            {
                MoveSpeed = 0;
                Runspeed = 0;
            }
            else
            {
                MoveSpeed = 2;
                Runspeed = 10;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Anim.Play("Parry");
                Anim.SetBool("CanMove", false);
            }

            if (CanMove == false)
            {
                Vector3 delta = Anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * 1) / Time.deltaTime;
                rigid.velocity = v;

                rigid.drag = 0;
            }
            else
            {
                return;
            }
        }

        public void FixedTick(float d)
        {
            Delta = d;
            DetectAction();
            Rolls();


            if (InAction)
            {
                Anim.applyRootMotion = true;

                _actionDelay += Delta;
                if (_actionDelay > 0.01f)
                {
                    InAction = false;
                    _actionDelay = 0;
                }
                else
                {
                    return;
                }
            }

            CanMove = Anim.GetBool("CanMove");

            if (!CanMove)
                return;

            Anim.applyRootMotion = false;


            rigid.drag = (MoveAount > 0 || OnGround == false) ? 0 : 4;

            float targetSpeed = MoveSpeed;
            if (run)
                targetSpeed = Runspeed;

            if (OnGround && CanMove)
                rigid.velocity = MoveDir * (targetSpeed * MoveAount);

            //  if (run)
            // LockOn = false;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Anim.SetBool("Crouch", true);
                Anim.SetFloat("IsCrouching", MoveAount, 0.4f, Delta);
            }
            else
            {
                Anim.SetBool("Crouch", false);
            }


            // Handle Rotation
            Vector3 TargtetedDir = (LockOn == false) ? MoveDir
                : (EnemyLockOnTransform != null) ? EnemyLockOnTransform.transform.position - transform.position
                : MoveDir;

            TargtetedDir.y = 0;
            if (TargtetedDir == Vector3.zero)
                TargtetedDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(TargtetedDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Delta * MoveAount * RotatioonSpeed);
            transform.rotation = targetRotation;
            Anim.SetBool("LockOn", LockOn);


            if (!LockOn)
                HandleMovementAnimations();
            else
                handleLockOnAnimation(MoveDir);
        }

        public void DetectAction()
        {
            // Checking the parry.
            if (ParryIsOn == true)
            {
                if (rt)
                {
                    CheckForParry();
                    CheckBossParry();
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CheckForBackStab();
            }


            if (!CanMove)
                return;


            if (rb == false && rt == false && lt == false && lb == false)
                return;
            string targetAnim = null;
            int r = Random.Range(0, Attacks.Length);
            targetAnim = Attacks[r];

            if (string.IsNullOrEmpty(targetAnim))
                return;

            if (rb)
                Anim.CrossFade(targetAnim, 0.01f);
            Anim.applyRootMotion = true;

            // For parrying player
            CanBeParried = true;
            CanMove = false;
            InAction = true;
        }

        public bool CheckForParry()
        {
            EnemyStates ParryTarget = null;


            Vector3 Orgin = transform.position;
            Orgin.y += 1;
            Vector3 RayDir = transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(Orgin, RayDir, out hit, 3f, IgnoreLayers))
            {
                ParryTarget = hit.transform.GetComponentInParent<EnemyStates>();
            }

            if (ParryTarget == null)
                return false;

            /*  if(ParryTarget.ParriedBY == null)
                  return false;*/


            /*    float distnace = Vector3.Distance(ParryTarget.transform.position,transform.position);
                if(distnace > 3f)
                
                    return false;*/

            Vector3 dir = ParryTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;
            float _angle = Vector3.Angle(transform.forward, dir);

            if (_angle < 60)
            {
                Vector3 targetPos = -dir * ParryOffset;
                targetPos += ParryTarget.transform.position;
                transform.position = targetPos;


                if (dir == Vector3.zero)
                    dir = -ParryTarget.transform.forward;
                Quaternion ERotation = Quaternion.LookRotation(-dir);
                Quaternion ourRot = Quaternion.LookRotation(dir);

                ParryTarget.transform.rotation = ERotation;
                transform.rotation = ourRot;
                ParryTarget.IsGettingParried();

                Anim.CrossFade("Parry", 0.01f);
                CanMove = false;
                InAction = true;
                ParryIsOn = false;
                EnemyTarget = null;


                return true;
            }

            return false;
        }

        public bool CheckBossParry()
        {
            BossStates ParryTarget = null;


            Vector3 Orgin = transform.position;
            Orgin.y += 1;
            Vector3 RayDir = transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(Orgin, RayDir, out hit, 3f, IgnoreLayers))
            {
                ParryTarget = hit.transform.GetComponentInParent<BossStates>();
            }

            if (ParryTarget == null)
                return false;


            Vector3 dir = ParryTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;
            float _angle = Vector3.Angle(transform.forward, dir);

            if (_angle < 60)
            {
                Vector3 targetPos = -dir * ParryOffset;
                targetPos += ParryTarget.transform.position;
                transform.position = targetPos;


                if (dir == Vector3.zero)
                    dir = -ParryTarget.transform.forward;
                Quaternion ERotation = Quaternion.LookRotation(-dir);
                Quaternion ourRot = Quaternion.LookRotation(dir);

                ParryTarget.transform.rotation = ERotation;
                transform.rotation = ourRot;
                ParryTarget.IsGettingParried();


                CanMove = false;
                InAction = true;
                ParryIsOn = false;
                EnemyTarget = null;


                return true;
            }

            return false;
        }

        public bool CheckForBackStab()
        {
            EnemyStates BackStab = null;

            Vector3 Orgin = transform.position;
            Orgin.y += 1;
            Vector3 RayDir = transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(Orgin, RayDir, out hit, 1f, IgnoreLayers))
            {
                BackStab = hit.transform.GetComponentInParent<EnemyStates>();
            }

            if (BackStab == null)
                return false;


            Vector3 dir = transform.position - BackStab.transform.position;
            dir.Normalize();

            dir.y = 0;
            float _angle = Vector3.Angle(BackStab.transform.forward, dir);

            if (_angle > 150)
            {
                Vector3 targetPos = dir * BackStabOffset;
                targetPos += BackStab.transform.position;
                transform.position = targetPos;

                Anim.Play("Stab");
                BackStab.transform.rotation = transform.rotation;
                BackStab.IsGettingBackStab();

                CanMove = false;
                InAction = true;

                // Need to change this shit..
                ParryIsOn = false;

                EnemyTarget = null;
                return true;
            }

            return false;
        }

        public void Rolls()
        {
            if (!RollInput)
            {
                col.enabled = true;
                return;
            }

            col.enabled = false;

            float v = Vertical;
            float h = Horizontal;
            v = (MoveAount > 0.3f) ? 1 : 0;
            h = 0;


            /* if (LockOn == false)
             {
                 v = (MoveAount > 0.3f)? 1 :0;
                 h = 0;
             } else
             {
                 if (Mathf.Abs(v) < 0.3f)
                     v = 0;
                 if (Mathf.Abs(h) < 0.3f)
                     h = 0;
             }*/
            if (v != 0)
            {
                if (MoveDir == Vector3.zero)
                    MoveDir = transform.forward;
                Quaternion tragetrot = Quaternion.LookRotation(MoveDir);
                transform.rotation = tragetrot;
            }

            Anim.SetFloat("Vertical", v);
            Anim.SetFloat("Horizontal", h);

            CanMove = false;

            InAction = true;

            Anim.CrossFade("Roll", 0.001f);
        }

        public void Tick(float d)
        {
            Delta = d;
            OnGround = On_Ground();
            // grounded animation.
        }

        public void HandleMovementAnimations()
        {
            Anim.SetBool("Run", run);

            Anim.SetFloat("Vertical", MoveAount, 0.4f, Delta);
        }


        public void Jump()
        {
            SkipGroundCheck = true;
            Anim.Play("IsJumping");
            rigid.velocity = rigid.velocity + Vector3.up * JumpSpeed;
            Anim.SetLayerWeight(Anim.GetLayerIndex("OverRide"), 0);
        }


        bool SkipGroundCheck;
        float SkipTimer;
        public bool PreviousGround;

        public bool On_Ground()
        {
            if (SkipGroundCheck == true)
            {
                OnGround = false;
                SkipTimer += Delta;
                if (SkipTimer > 0.2f)
                    SkipGroundCheck = false;
                PreviousGround = false;
                OnGround = true;
                return false;
            }

            SkipTimer = 0f;

            bool r = false;
            Vector3 orgin = transform.position + (Vector3.up * DistanceToGround);
            Vector3 dir = -Vector3.up;
            float dis = DistanceToGround + DistoGr;
            Debug.DrawRay(orgin, dir * dis);

            RaycastHit hit;
            if (Physics.Raycast(orgin, dir, out hit, dis, IgnoreLayers))
            {
                r = true;
                Vector3 targetpostion = hit.point;
                transform.position = targetpostion;
            }

            if (r == true && !PreviousGround)
            {
                Anim.Play("Landing");
                Anim.SetLayerWeight(Anim.GetLayerIndex("OverRide"), 1);
            }


            PreviousGround = r;


            return r;
        }

        public void handleLockOnAnimation(Vector3 MoveDir)
        {
            Vector3 RelativeDirection = transform.InverseTransformDirection(MoveDir);
            float H = RelativeDirection.x;
            float V = RelativeDirection.z;
            Anim.SetFloat("Vertical", V, 0.2f, Delta);
            Anim.SetFloat("Horizontal", H, 0.2f, Delta);
        }


        public void Sprint()
        {
            if (!run)
                return;
            float v = Vertical;
            float h = Horizontal;
            v = (MoveAount > 0.3f) ? 1 : 0;
            h = 0;

            if (v != 0)
            {
                if (MoveDir == Vector3.zero)
                    MoveDir = transform.forward;
                Quaternion tragetrot = Quaternion.LookRotation(MoveDir);
                transform.rotation = tragetrot;
            }

            Anim.SetFloat("Vertical", v);
            Anim.SetFloat("Horizontal", h);

            CanMove = false;

            InAction = true;

            Anim.SetBool("Run", true);
        }
    }
}