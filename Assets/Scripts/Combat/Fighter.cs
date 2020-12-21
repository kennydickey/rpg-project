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
        float timeSinceLastAttack = Mathf.Infinity; //makes greater than always true

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
                TriggerAttack();
                //reset time to 0 and go again
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            //clean up of trigger before starting again v
            GetComponent<Animator>().ResetTrigger("attack"); 
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event (default placeholder)
        void Hit()
        {
            if(target == null) { return; }
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            //true if our position is less than weapon range
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            //return true if..
            return targetToTest != null && !targetToTest.IsDead(); //continuing from previous line
        }

        public void Attack(GameObject combatTarget)
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);
            print("take that");
            target = combatTarget.GetComponent<Health>();  //previously combatTarget.transform      
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            //clean up of trigger before starting again v
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
