using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{



    public class ActionScheduler : MonoBehaviour
    {

        IAction currentAction; //starts as null

        public void StartAction(IAction action)
        {
            if (currentAction == action) return; //preventing same action from happening in succession
            if (currentAction != null)
            {
                currentAction.Cancel();
                print("canceling " + currentAction);
            }
            currentAction = action; //actually updating the action here
        }
    }


}