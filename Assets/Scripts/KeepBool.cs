using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBool : StateMachineBehaviour
{

    public string BoolName;
    public bool Status;
    public bool ResetonExit = true;
  
   
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetBool(BoolName, Status);


        
    }


    public override void OnStateExit(Animator animnator, AnimatorStateInfo stateInfo,int layerIndex)
    {
        if(ResetonExit)
        animnator.SetBool(BoolName, !Status);
    }
    
}
