using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Resources
{

    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;

        //method to award xp
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }
    }

}