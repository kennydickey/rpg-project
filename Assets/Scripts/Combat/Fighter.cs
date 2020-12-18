using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f; //once every second
        [SerializeField] float weaponDamage = 5f;

        Transform target;
        float timeSinceLastAttack = 0;

        private void Update()
        {
            //increment on each update
            timeSinceLastAttack += Time.deltaTime; //time that last frame took to render

            if (target == null) return; //while not moving to target nor stopping
            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the hit() event
                GetComponent<Animator>().SetTrigger("attack");
                //reset time to 0 and go again
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event
        void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            //true if our position is less than weapon range
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);
            print("take that");
            target = combatTarget.transform;        
        }

        public void Cancel()
        {
            target = null;
        }


    }
}
