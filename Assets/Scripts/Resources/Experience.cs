using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Saving;

namespace RPG.Resources
{

    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;      

        //method to award xp
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }


        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state; // casting the state to ensure we get a float back
        }
    }

}