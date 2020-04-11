using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class ActionManager : MonoBehaviour
    {


        /*  public List<Action> actionSlots = new List<Action>();


          public void Init()
          {


          }

          ActionManager ()
          {
              for (int i = 0; i < 4; i++)
              {

                  Action a = new Action();
                  a.input = (ActionInput)i;
                  actionSlots.Add(a);
              }

          }

          public Action GetactionAnim(StateManager st)
          {

              ActionInput A_input = GetAction(st);
              return GetActionEntry(A_input);


          }


          Action GetActionEntry(ActionInput _input)
          {

              for (int i = 0; i < actionSlots.Count; i++)
              {
                  if (actionSlots[i].input == _input)
                      return actionSlots[i];
              }
              return null;
          }
          public ActionInput GetAction(StateManager st)
          {



              if (st.rb)
                  return ActionInput.rb;

              if (st.Q)
                  return ActionInput.Q;

              return ActionInput.rb;
          }






      }

      public enum ActionInput
      {

          rb , Q 
      }
      [System.Serializable]
      public class Action
      {
          public ActionInput input;

          public string targetAnim;


      }*/

    }
}
