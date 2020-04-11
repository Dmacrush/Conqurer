using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace SA
{


    public class BossStates : MonoBehaviour
    {
        [Header("Stats")]
        public GameObject ParriedEffects;
        public GameObject ParriedSpark;
        public Transform EffectsPoint;
        public Image BossDamageEffects;
        public Image EnemyHealth;
        public GameObject Barrier;
        public NavMeshAgent AgentAI;
        public bool hasDestination;
        public Vector3 TargetDestination;
        public Vector3 DirtoTarget;
        public bool RotateToTarget;


        [Header("Animations Names")]
        public string EnemyCanMoveBool;
        public string Can_move;
        public string GettingHurt;



        [Header("Values")]
        public float Horizontal;
        public float Vertical;
        public float delta;
        public Vector3 TargetDir;



        [Header("States")]
        public GameObject Clip;
        public bool CanMove;
        public bool CanBeParryied = true;
        public bool EparryIsON = true;
        public bool DontDoAnything;
        public bool isInvincible;
        public bool isDead;
        StateManager ParriedBY;
        //public EnemyAnimationHook E_hook;
        BossTarget ETarget;
        public Rigidbody RIGI;
        public Animator anim;
        public LayerMask IgnoreLayers;

        List<Rigidbody> RagDollsRigids = new List<Rigidbody>();
        List<Collider> ragDollColliders = new List<Collider>();


        float timer;


        public void Init()
        {




        }
        public void SetDestination(Vector3 d)
        {
            if (!hasDestination)
            {
                hasDestination = true;
                AgentAI.isStopped = false;
                AgentAI.SetDestination(d);
                TargetDestination = d;
            }
        }
        public void Start()
        {
            AgentAI = GetComponent<NavMeshAgent>();
            RIGI.isKinematic = true;

            RIGI = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();

            ETarget = GetComponent<BossTarget>();
            InitRagDoll();
            EparryIsON = false;
            IgnoreLayers = ~(1 << 9);

        }


        private void Update()
        {
            

            if(EnemyHealth.fillAmount < BossDamageEffects.fillAmount)
            {
                Invoke("BossHealth", 1f);

            }

        }

        public void Tick(float D)
        {
            delta = D;

            CanMove = anim.GetBool(EnemyCanMoveBool);

            if (DontDoAnything)
            {
                DontDoAnything = !CanMove;
                return;
            }

            if (EnemyHealth.fillAmount <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                    Barrier.SetActive(false);
                    EnableRagDoll();
                    PlayerStats player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
                    player.PlayerSkillPoint(0.5f);
                }
            }

            if (isInvincible)
            {

                isInvincible = !CanMove;
            }

            if (ParriedBY != null && EparryIsON == false)
            {
                //   ParriedBY.ParryTarget = null;
                ParriedBY = null;

            }

            if (RotateToTarget)
                LookTowardsPlayer();

            if (CanMove)
            {
                anim.applyRootMotion = true;
                // MoveAnimations();


            }


        }


        public void DoDamage(float v)
        {
            if (isInvincible)
                return;
            EnemyHealth.fillAmount -= v;
            isInvincible = true;
            anim.applyRootMotion = true;
            CanMove = false;
            anim.Play(GettingHurt);

        }
        void LookTowardsPlayer()
        {

            Vector3 dir = DirtoTarget;
            dir.y = 0;
            if (dir == Vector3.zero)
                dir = transform.forward;
            Quaternion TargetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, delta * 5);
        }

        public void MoveAnimations()
        {
            /*  Vector3 desireVel = AgentAI.desiredVelocity;
              Vector3 relative = transform.InverseTransformDirection(desireVel);

              float v = relative.z;
              float h = relative.x;

              anim.SetFloat(0, h, 0.2f, delta);
             // anim.SetFloat(v, 0, 0.2f, delta);*/
            anim.SetFloat(Can_move, 1);




        }

        void InitRagDoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i] == RIGI)
                    continue;

                RagDollsRigids.Add(rigs[i]);
                rigs[i].isKinematic = true;
                Collider col = rigs[i].gameObject.GetComponent<Collider>();
                col.isTrigger = true;
                ragDollColliders.Add(col);
            }

        }


        public void EnableRagDoll()
        {

            for (int i = 0; i < RagDollsRigids.Count; i++)
            {

                RagDollsRigids[i].isKinematic = false;
                ragDollColliders[i].isTrigger = false;
            }

            Collider controllerCOliider = RIGI.gameObject.GetComponent<Collider>();
            controllerCOliider.enabled = false;
            RIGI.isKinematic = true;
            StartCoroutine("CloseAnimator");

        }


        IEnumerator CloseAnimator()
        {
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            this.enabled = false;
        }


        public void CheckForParry(Transform Target, StateManager STATES)
        {
            if (CanBeParryied == false || STATES.ParryIsOn == false || isInvincible)
                return;

            Vector3 dir = transform.position - Target.position;
            dir.Normalize();
            float dot = Vector3.Dot(Target.forward, dir);
            if (dot < 0)
                return;


            CanMove = false;

            isInvincible = true;
            anim.Play(GettingHurt);
            anim.applyRootMotion = true;
            // STATES.ParryTarget = this;
            ParriedBY = STATES;
            return;



        }

        public void IsGettingParried()
        {

            EnemyHealth.fillAmount -= 0.05f;
            DontDoAnything = true;
            CanMove = false;
            anim.Play(GettingHurt);
            anim.applyRootMotion = true;
            GameObject C = Instantiate(Clip, transform.position, transform.rotation);
            Destroy(C, 1f);
            GameObject E = Instantiate(ParriedEffects, EffectsPoint.position, EffectsPoint.rotation);
            Destroy(E, 2f);

            GameObject S = Instantiate(ParriedSpark, EffectsPoint.position, EffectsPoint.rotation);
            Destroy(S, 2f);

        }

        public void BossHealth()
        {

            BossDamageEffects.fillAmount = EnemyHealth.fillAmount;

        }
     
    }

}