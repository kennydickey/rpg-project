using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{



    public class ActionScheduler : MonoBehaviour
    {

        MonoBehaviour currentAction; //starts as null

        public void StartAction(MonoBehaviour action)
        {
            if (currentAction == action) return; //preventing same action from happening in succession
            if (currentAction != null)
            {
            print("canceling " + currentAction);
            }
            currentAction = action; //actually updating the action here
        }
    }


}