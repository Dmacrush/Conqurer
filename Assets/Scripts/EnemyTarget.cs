using System;
using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public int Index;
    public List<Transform> Enemylist = new List<Transform>();
    public List<HumanBodyBones> Human_Bones = new List<HumanBodyBones>();
    public EnemyStates eSTATES;
    Animator anime;
        
    public GameObject lockOnRetical;
    public bool lockOn;
        
    public void Init(EnemyStates st)
    {
        lockOnRetical.SetActive(false);
            
        eSTATES = st;
        anime = eSTATES.anim;

        if (anime.isHuman == false)
            return;
        for (int i = 0; i < Human_Bones.Count; i++)
        {
            Enemylist.Add(anime.GetBoneTransform(Human_Bones[i]));
        }

        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.enemyTargets.Add(this);
    }

    public void Update()
    {
        if (lockOn)
        {
            LockOnTrue();
        }
        else
        {
            LockOnFalse();
        }
    }

    public void LockOnTrue()
    {
        lockOnRetical.SetActive(true);
    }

    public void LockOnFalse()
    {
        lockOnRetical.SetActive(false);
    }

    public Transform GetTarget(bool negative = false)
    {
        if (Enemylist.Count == 0)
            return transform;

//            int EnemyIndex = Index;

        if (!negative)
        {
            if (Index < Enemylist.Count - 1)
                Index++;
            else
                Index = 0;
        }
        else
        {
            Index--;

            if (Index < 0)
                Index = Enemylist.Count - 1;
        }

        //  Index = Mathf.Clamp(Index, 0, Enemylist.Count);
        return Enemylist[Index];
    }
}