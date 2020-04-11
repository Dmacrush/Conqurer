using System;
using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStates : MonoBehaviour
{
    [Header("Stats")] public GameObject SkillPoints;
    public GameObject ParriedEffects;
    public GameObject ParriedSpark;
    public Transform EffectsPoint;
    public Image EnemyHealth;
    public Image EnemyDamage;
    public NavMeshAgent AgentAI;
    public bool hasDestination;
    public Vector3 TargetDestination;
    public Vector3 DirtoTarget;
    public bool RotateToTarget;

    public Vector3 EnemyPos;


    [Header("Animations Names")] public string EnemyCanMoveBool;
    public string Can_move;
    public string AttackInterupt;
    public string BackStab;
    public string GettingHurt;
    public string Block;


    [Header("Values")] public float Horizontal;
    public float Vertical;
    public float delta;
    public Vector3 TargetDir;


    [Header("States")] public GameObject Clip;
    public bool CanMove;
    public bool CanBeParryied = true;
    public bool EparryIsON = true;
    public bool DontDoAnything;
    public bool isInvincible;
    public bool isDead;

    StateManager ParriedBY;

    //public EnemyAnimationHook E_hook;
    EnemyTarget ETarget;
    public Rigidbody RIGI;
    public Animator anim;
    public LayerMask IgnoreLayers;

    List<Rigidbody> RagDollsRigids = new List<Rigidbody>();
    List<Collider> ragDollColliders = new List<Collider>();

    private int defaultHealth;
    public Vector3 defaultPosition;

    private EnemyAIHandler AiHandler;

    float timer;

    public void Awake()
    {
        SubToEvent();
        defaultPosition = transform.position;
        Initialization();
        if (AiHandler == null)
            AiHandler = GetComponent<EnemyAIHandler>();
    }

    private void SubToEvent()
    {
        FindObjectOfType<PlayerStats>().deathEvent += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        FindObjectOfType<PlayerStats>().deathEvent -= OnPlayerDeath;
        SubToEvent();
        transform.position = defaultPosition;

        SetDestination(EnemyPos);
        isDead = false;

        Initialization();


        DisableRagDoll();
        EnemyHealth.fillAmount = 1;
    }

    void DisableRagDoll()
    {
        for (int i = 0; i < RagDollsRigids.Count; i++)
        {
            RagDollsRigids[i].isKinematic = true;
            ragDollColliders[i].isTrigger = true;
        }

        Collider controllerCOliider = RIGI.gameObject.GetComponent<Collider>();
        controllerCOliider.enabled = true;
        RIGI.isKinematic = false;
        StartCoroutine(OpenAnimator());
    }

    IEnumerator OpenAnimator()
    {
        yield return new WaitForEndOfFrame();
        anim.enabled = true;
        this.enabled = true;
    }

    private void Update()
    {
        if (EnemyHealth.fillAmount < EnemyDamage.fillAmount)
        {
            Invoke("EnemyDamageBar", 1f);
        }


        if (FindObjectOfType<PlayerStats>().PlayerHealth.fillAmount <= 0)
        {
            anim.Play("MoveBack");
            AgentAI.SetDestination(EnemyPos);
        }

        if (Vector3.Distance(transform.position, EnemyPos) < 1f)
        {
            AgentAI.isStopped = true;
        }
    }


    void EnemyDamageBar()
    {
        EnemyDamage.fillAmount = EnemyHealth.fillAmount;
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


    public void Initialization()
    {
        AgentAI = GetComponent<NavMeshAgent>();
        RIGI.isKinematic = true;

        RIGI = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        ETarget = GetComponent<EnemyTarget>();
        ETarget.Init(this);
        InitRagDoll();
        EparryIsON = false;
        IgnoreLayers = ~(1 << 9);
        EnemyPos = this.transform.position;
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
                EnableRagDoll();
                Invoke("SkillPoint", 5f);
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
        anim.Play(AttackInterupt);
        anim.applyRootMotion = true;
        // STATES.ParryTarget = this;
        ParriedBY = STATES;
        return;
    }

    public void IsGettingParried()
    {
        GameObject E = Instantiate(ParriedEffects, EffectsPoint.position, EffectsPoint.rotation);
        Destroy(E, 2f);

        GameObject S = Instantiate(ParriedSpark, EffectsPoint.position, EffectsPoint.rotation);
        Destroy(S, 2f);


        EnemyHealth.fillAmount -= 0.05f;
        DontDoAnything = true;
        CanMove = false;
        anim.Play(AttackInterupt);
        anim.applyRootMotion = true;
        GameObject C = Instantiate(Clip, transform.position, transform.rotation);
        Destroy(C, 1f);
    }


    public void IsGettingBackStab()
    {
        CanMove = false;

        EnemyHealth.fillAmount -= 1f;
        DontDoAnything = true;
        anim.applyRootMotion = true;
        anim.Play(BackStab);
    }


    public void SkillPoint()
    {
        Instantiate(SkillPoints, transform.position, transform.rotation);
    }
}