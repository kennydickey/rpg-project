using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable //implemet ISaveable interface
    {

        [SerializeField] float healthPoints = 20f;

        bool isDead = false;       

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0); //returns whichever is greatest of these nums
            print(healthPoints);
            if (healthPoints == 0)
            { 
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return; //exit fn
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
