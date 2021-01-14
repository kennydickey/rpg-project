﻿using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable //implemet ISaveable interface
    {

        [SerializeField] float healthPoints = 20f;

        bool isDead = false;

        // todo fix health resetting in start between scenes
        private void Start()
        {
            // health gets info from base stats at start
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator,  float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0); //returns whichever is greatest of these nums
            print(healthPoints);
            if (healthPoints == 0)
            { 
                Die();
                AwardExperience(instigator);
            }
        }       

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetHealth());
        }

        private void Die()
        {
            if (isDead) return; //exit fn
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        public object CaptureState()
        {
            return healthPoints;
        }
        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0) // char must die again after load
            {
                Die();
            }
        }

    }
}