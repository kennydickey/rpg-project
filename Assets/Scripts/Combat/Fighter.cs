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

        // more specifi, and gives us access to Health methods and such
        Health target; //previously Transform target
        float timeSinceLastAttack = 0;

        private void Update()
        {
            //increment on each update
            timeSinceLastAttack += Time.deltaTime; //time that last frame took to render

            if (target == null) return; //while not moving to target nor stopping
            if (target.IsDead() == true) //remember.. we have target as Health script already
            {
                return;
            }

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the hit() event
                GetComponent<Animator>().SetTrigger("attack");
                //reset time to 0 and go again
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event (default placeholder)
        void Hit()
        {
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            //true if our position is less than weapon range
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            //return true if..
            return targetToTest != null && !targetToTest.IsDead(); //continuing from previous line
        }

        public void Attack(CombatTarget combatTarget)
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);
            print("take that");
            target = combatTarget.GetComponent<Health>();  //previously combatTarget.transform      
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }


    }
}
