using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable //implemet ISaveable interface
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] float healthPoints = 20;
        // unserialized, not needed
        //float healthPoints = -1f; // restored healthpoints will never be negative

        bool isDead = false;

        // todo fix health resetting in start between scenes
        private void Start()
        {
            // subscribing to onLevelUp, so this will happen when it is called in BaseStats
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

            if(healthPoints < 0) // <0 restore, =0 stay dead
            {
                // health gets info from base stats at start
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
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
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
                float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints); //return greater of two vals

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
