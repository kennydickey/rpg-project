using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{

    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate(); // void type delegate, often used
        //public event ExperienceGainedDelegate onExperienceGained; // new delegate instance
        //replace with..
        public event Action onExperienceGained; // Action is a predefined delegate with no return type

        //method to award xp
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained(); //calls everything in delegate list
        }

        public float GetPoints()
        {
            return experiencePoints;
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