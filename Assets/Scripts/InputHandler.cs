using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        public bool lockOn;

        WeaponHook H_Hook;

        float Vertical;
        float Horizontal;
        bool MiddleMOuse;
        float Delta;


        public bool B_Input;
        bool A_Input;
        bool x_Input;
        bool y_input;

        bool RB_input;
        float RT_Axis;
        bool RT_input;
        bool LB_input;
        float LT_Axis;
        bool LT_input;


        float B_trimer;


        EnemyStates Estates;
        StateManager States;
        CameraManager CamManager;

        private void Awake()
        {
            H_Hook = GetComponent<WeaponHook>();
            States = GetComponent<StateManager>();
            States.Init();
            CamManager = CameraManager.Singleton;
            CamManager.Init(States);
            SubToEvent();
        }

        private void SubToEvent()
        {
            FindObjectOfType<PlayerStats>().deathEvent += OnPlayerDeath;
        }

        void OnPlayerDeath()
        {
            //disable lock on after death
            States.LockOn = false;
            CamManager.LockOn = false;
            States.EnemyTarget = null;
            States.EnemyLockOnTransform = null;
            CamManager.LockOnTarget = null;

            FindObjectOfType<PlayerStats>().deathEvent -= OnPlayerDeath;
            SubToEvent();
        }

        private void FixedUpdate()
        {
            Delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            States.FixedTick(Delta);
            CamManager.Tick(Delta);


            if (B_Input == false)
                B_trimer = 0;

            if (States.RollInput)
                States.RollInput = false;
            if (States.run)
                States.run = false;
        }

        private void Update()
        {
            Delta = Time.deltaTime;
            States.Tick(Delta);

            ResetInputAndStates();
        }

        public void GetInput()
        {
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");
            B_Input = Input.GetKey(KeyCode.Q);
            MiddleMOuse = Input.GetKeyDown(KeyCode.Mouse2);
            RT_input = Input.GetKey(KeyCode.Mouse1);
            RT_Axis = Input.GetAxis("RT");

            if (RT_Axis != 0)
                RT_input = true;


            LT_input = Input.GetButton("LT");
            LT_Axis = Input.GetAxis("LT");

            if (LT_Axis != 0)
                LT_input = true;

            RB_input = Input.GetKeyDown(KeyCode.Mouse0);
            LB_input = Input.GetButton("LB");

            if (B_Input)
                B_trimer += Delta;
        }

        void UpdateStates()
        {
            States.Horizontal = Horizontal;
            States.Vertical = Vertical;
            Vector3 v = Vertical * CamManager.transform.forward;
            Vector3 h = Horizontal * CamManager.transform.right;
            States.MoveDir = (v + h).normalized;
            float m = Mathf.Abs(Horizontal) + Mathf.Abs(Vertical);
            States.MoveAount = Mathf.Clamp01(m);
            States.RollInput = B_Input;
            States.run = Input.GetKey(KeyCode.LeftShift);


            if (B_Input && B_trimer > 0.5f)
            {
                States.run = (States.MoveAount > 0);
            }

            if (B_Input == false && B_trimer > 0 && B_trimer < 0.5f)
            {
                States.RollInput = true;
            }

            if (States.EnemyTarget != null)
            {
                if (States.EnemyTarget.eSTATES.isDead)
                {
                    States.LockOn = false;
                    CamManager.LockOn = false;
                    States.EnemyTarget = null;
                    States.EnemyLockOnTransform = null;
                    CamManager.LockOnTarget = null;
                }
            }

            else
            {
                States.LockOn = false;
                CamManager.LockOn = false;
                States.EnemyTarget = null;
                States.EnemyLockOnTransform = null;
                CamManager.LockOnTarget = null;
            }


            if (MiddleMOuse)
            {
                States.LockOn = !States.LockOn;
                CamManager.LockedOn = true;
                States.EnemyTarget = EnemyManager.Singleton.GetEnemy(transform.position);
                //States.BossTarget = BossManager.Singleton.GetEnemy(transform.position);

                if (States.EnemyTarget == null)
                {
                    States.LockOn = false;
                    CamManager.LockOnTarget = States.EnemyTarget;
                    States.EnemyLockOnTransform = States.EnemyTarget.GetTarget();
                    CamManager.LockonTransform = States.EnemyLockOnTransform;
                    CamManager.LockOn = States.LockOn;
                }

                /* CamManager.BLockOnTarget = States.BossTarget;
                 States.EnemyLockOnTransform = States.BossTarget.GetTraget();
                 CamManager.LockonTransform = States.EnemyLockOnTransform;
                 CamManager.LockOn = States.LockOn;*/
            }
            else
            {
                if (CamManager.LockOnTarget != null) CamManager.LockOnTarget.lockOn = true;
            }

            States.rt = RT_input;
            States.lt = LT_input;
            States.rb = RB_input;
            States.lb = LB_input;
        }


        void ResetInputAndStates()
        {
            if (B_Input == false)
                B_trimer = 0;

            if (States.RollInput)
                States.RollInput = false;
            if (States.run)
                States.run = false;
        }

        public void DamageColliderOpen()
        {
            if (H_Hook != null)
            {
                H_Hook.OpenDamageCollider();
            }


            OpenParryFlags();
        }

        public void DamageColliderClose()
        {
            if (H_Hook != null)
            {
                H_Hook.CloseDamageCollider();
            }


            CloseParryFlags();
        }

        public void OpenParryFlags()
        {
            if (Estates)
            {
                Estates.EparryIsON = true;
            }

            if (States)
            {
                States.ParryIsOn = true;
            }
        }

        public void CloseParryFlags()
        {
            if (Estates)
            {
                Estates.EparryIsON = false;
            }

            if (States)
            {
                States.ParryIsOn = false;
            }
        }
    }
}