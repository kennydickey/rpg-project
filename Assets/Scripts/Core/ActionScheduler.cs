using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{



    public class ActionScheduler : MonoBehaviour
    {

        IAction currentAction; //starts as null

        //since Cancel() belongs to IAction interface, it will have the properties of whatever class is using StartAction() at the time
        public void StartAction(IAction action) //action is the current class being called
        {
            if (currentAction == action) return; //preventing same action from happening in succession
            if (currentAction != null)
            {
                currentAction.Cancel();
                //for fighter.cs, target = null
                //for mover.cs, navMeshAgent.isStopped = true;
                print("canceling " + currentAction);
            }
            currentAction = action; //actually updating the action here
        }
    }


}