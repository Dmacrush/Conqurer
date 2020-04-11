using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{


    public class BossAI : MonoBehaviour
    {

        [Header("Strings")]
        public string Idle;
        public string EnemyMove;
        public string BLock;
        public string Standingup;

        public BossAttacks[] ai_attacks;
        public GameObject BossHealth;
        public BossStates States;

        public Vector3 PLAYERPOS;
        public Vector3 EnemyPos;


        public StateManager _States;
        public Transform target;
        public float Sight;
        public float FOV_Angle;
        public int frameCount = 30;
        public int AttackCount = 30;
        int _AttackCount;


        int _Frame;

        float Dis;
        float _angle;
        float Delta;
        Vector3 DirToTarget;



        public int CloseCOunt = 10;
        int _Close;

        public float DistanceFromTarget()
        {


            if (target == null)
                return 1;
            return Vector3.Distance(target.position, transform.position);

        }
        public float AngleTotarget()
        {
            float a = 180;

            if (target)
            {
                Vector3 d = DirToTarget;
                a = Vector3.Angle(d, transform.forward);
            }
            return a;
        }

        private void Start()
        {
            EnemyPos = this.transform.position;
            if (States == null)
                States = GetComponent<BossStates>();
            States.Start();
        }



        public AIstates _AiStates;
        public enum AIstates
        {
            far, close, InSight, Attacking, death
        }
        private void Update()
        {
            Delta = Time.deltaTime;
            Dis = DistanceFromTarget();
            _angle = AngleTotarget();

            if (target)
                DirToTarget = target.position - transform.position;


            if (Dis > Sight || _angle > FOV_Angle)
            {
                _AiStates = AIstates.far;
                BossHealth.SetActive(false);
                States.anim.Play(Idle);
                States.AgentAI.isStopped = true;

            }

            // checking player pos // updating.
            if (Dis < Sight)
            {
                PLAYERPOS = target.position;
            }

            handleDeath();


            // checking dist is smaller

            if (Vector3.Distance(transform.position, PLAYERPOS) < 1f)
            {
                States.anim.SetFloat(EnemyMove, -1);

            }

            States.DirtoTarget = DirToTarget;

            switch (_AiStates)
            {
                case AIstates.far:
                    HandleFarSight();
                    break;
                case AIstates.close:
                    HandleCloseSight();
                    break;
                case AIstates.InSight:
                    In_Sight();
                    BossHealth.SetActive(true);
                    break;
                case AIstates.Attacking:
                    if (States.CanMove)
                    {
                        _AiStates = AIstates.InSight;
                        States.RotateToTarget = true;
                        States.anim.SetFloat(EnemyMove, -1);

                    }



                    break;

                case AIstates.death:
                    handleDeath();
                    break;

                default:
                    break;
            }

            States.Tick(Delta);
        }






        void GoToTarget()
        {
            States.hasDestination = false;
            States.SetDestination(target.position);
            States.anim.SetFloat(EnemyMove,1);

        }

        public void In_Sight()
        {

            States.RotateToTarget = true;
            HandleCoolDown();
            float disToDes = Vector3.Distance(States.TargetDestination, target.position);

            if (disToDes > 2 || Dis > Sight * 0.5)
             
                GoToTarget();
          

            if (Dis < 2)
                States.AgentAI.isStopped = true;



            if (_AttackCount > 0)
            {
                _AttackCount--;
                return;
            }
            _AttackCount = AttackCount;


            BossAttacks a = WillAttack();

            if (a != null)
            {

                States.anim.applyRootMotion = true;
                //  States.AgentAI.enabled = false;
                _AiStates = AIstates.Attacking;
                States.anim.Play(a.targetAnim);
                States.CanMove = false;
                a.cool = a.CoolDown;
                States.AgentAI.isStopped = true;
                return;
            }


        }


        public BossAttacks WillAttack()
        {
            int w = 0;
            List<BossAttacks> l = new List<BossAttacks>();
            for (int i = 0; i < ai_attacks.Length; i++)
            {
                BossAttacks a = ai_attacks[i];

                if (a.cool > 0)
                {
                    States.AgentAI.isStopped = true;

                    continue;
                }
                States.AgentAI.isStopped = false;

                // States.MoveAnimations();
                //   GoToTarget();

                if (Dis > a.MinDistance)
                    continue;
                if (_angle < a.minAngle)
                    continue;
                if (_angle > a.maxAngle)
                    continue;
                if (a.Weight == 0)
                    continue;
                w += a.Weight;
                l.Add(a);
            }
            if (l.Count == 0)
                return null;

            int ran = Random.Range(0, w + 1);
            int C_W = 0;
            for (int i = 0; i < l.Count; i++)
            {

                C_W += l[i].Weight;
                if (C_W > ran)
                {
                    return l[i];
                }

            }

            return null;
        }

        public void RayCastToTarget()
        {

            RaycastHit hit;
            Vector3 Origin = transform.position;
            Origin.y += 0.5f;
            Vector3 dir = DirToTarget;
            dir.y += 0.5f;
            if (Physics.Raycast(Origin, dir, out hit, Sight, States.IgnoreLayers))
            {

                StateManager st = hit.transform.GetComponent<StateManager>();
                if (st != null)
                {
                    States.anim.Play(Standingup);
                    _AiStates = AIstates.InSight;
                    States.RotateToTarget = true;
                 //   States.SetDestination(target.position);
                }

            }

        }

        public void HandleFarSight()
        {
            if (target == null)
                return;
            _Frame++;
            if (_Frame > frameCount)
            {
                _Frame = 0;
                if (Dis < Sight)
                {
                    if (_angle < FOV_Angle)
                    {

                        _AiStates = AIstates.close;
                    }
                }


            }



        }

        public void HandleCloseSight()
        {

            _Close++;
            if (_Close > CloseCOunt)
            {
                _Close = 0;
                if (Dis > Sight || _angle > FOV_Angle)
                {
                    _AiStates = AIstates.far;
                    return;

                }
            }

            RayCastToTarget();
        }

        void HandleCoolDown()
        {


            for (int i = 0; i < ai_attacks.Length; i++)
            {
                BossAttacks a = ai_attacks[i];

                if (a.cool > 0)
                {
                    //States.anim.SetFloat("EenmyMOve", -1);
                    States.anim.Play(BLock);
                    a.cool -= Delta;
                    if (a.cool < 0)
                    a.cool = 0;
                }
            }
        }

        public void handleDeath()
        {

            if (States.EnemyHealth.fillAmount <= 0)
            {
                States.RotateToTarget = false;
                States.AgentAI.isStopped = true;
                _AiStates = AIstates.death;
            }

        }


        /*   public void MoveBackToOriginalPos()
           {
               States.AgentAI.SetDestination(EnemyPos);
               States.AgentAI.speed = 0.7f;
               States.anim.Play("MoveBack");
           }*/


      
    }

    [System.Serializable]
    public class BossAttacks
    {

        public int Weight;
        public float MinDistance;
        public float minAngle;
        public float maxAngle;

        public float CoolDown = 2;
        public float cool;
        public string targetAnim;

    }
}
