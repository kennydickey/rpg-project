using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Resources
{
    public class Health : MonoBehaviour//, ISaveable //implemets ISaveable interface
    {
        [SerializeField] float regenerationPercentage = 70;
        //[SerializeField] float healthPoints = 20;
        // unserialized, not needed
        //float healthPoints = -1f; // restored healthpoints will never be negative
        [SerializeField] UnityEvent takeDamage;

        // using LazyValue script
        LazyValue<float> healthPoints;

        bool isDead = false;

        private void Awake()
        {
            // initialized before first use but GetInitialHealth() is not called until needed
            healthPoints = new LazyValue<float>(GetInitialHealth); //GetInitialHealth is a delegate
        }

        private float GetInitialHealth() // < this is a delegate
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        // todo fix health resetting in start between scenes
        private void Start()
        {
            // if we have not accessed healthPoints GetinitialHealth() and caused it to initialize, it will now do so
            healthPoints.ForceInit();
            //if(healthPoints < 0) // <0 restore, =0 stay dead
            //{
            //    // health gets info from base stats at start
            //    healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            //}

        }

        private void OnEnable()
        {
            // subscribing to onLevelUp, so this will happen when it is called in BaseStats
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator,  float damage)
        {
            print(gameObject.name + "took damage " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); //returns whichever is greatest of these nums
            if (healthPoints.value == 0)
            { 
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(); //triggers all functions specified in unity inspector
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            // forwarding?
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints); //return greater of two vals

        }

        public object CaptureState()
        {
            return healthPoints; //healthpoints is a float and therefore serializeable
        }
        public void RestoreState(object state)
        {
            healthPoints.value = (float)state; // cast to make sure that our state is a float

            if (healthPoints.value <= 0) // char must die again after load
            {
                Die();
            }
        }

    }
}
