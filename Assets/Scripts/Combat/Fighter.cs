﻿using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider //, ISaveable
    {

        [SerializeField] float timeBetweenAttacks = 1f; //once every second
        [SerializeField] Transform rightHandTransform = null; //where to place
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null; // to place whichever weapon
        //[SerializeField] string defaultWeaponName = "Unarmed";

        // more specifi, and gives us access to Health methods and such
        Health target; //previously Transform target
        float timeSinceLastAttack = Mathf.Infinity; //makes greater than always true
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        GameObject player;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            //looks in Resources folder for obj with type weapon named "unarmed"
            //if (currentWeapon == null) { } // so that we do not override what saving system has
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            //increment on each update
            timeSinceLastAttack += Time.deltaTime; //time that last frame took to render

            if (target == null) return; //while not moving to target nor stopping
            if (target.IsDead() == true) //remember.. we have target as Health script already
            {
                return;
            }

            if (!GetIsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            //if (weapon == null) return; // no longer needed
            currentWeapon.value =  AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
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

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeaponConfig.hasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }           
        }
        void Shoot() //for calling shoot anim specifically
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            //true if our position is less than weapon range
            return Vector3.Distance(transform.position, targetTransform.transform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform))
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            //return true if..
            return targetToTest != null && !targetToTest.IsDead(); //continuing from previous line
        }
        public bool PlayerCanAttack(GameObject combatTarget)
        {
            if (combatTarget == null || combatTarget == player) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            //return true if..
            return targetToTest != null && !targetToTest.IsDead(); //continuing from previous line
        }

        public void Attack(GameObject combatTarget)
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);          
            target = combatTarget.GetComponent<Health>();  //previously combatTarget.transform      
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            //clean up of trigger before starting again v
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }


        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            // if stat in question passed in above == Stat.cs.damage
            if(stat == Stat.Damage)
            {
                // additive modifier on top of character dameage
                yield return currentWeaponConfig.GetWeaponDamage();
                // yield return secondWeapon damage, can do multiple yield returns
            }
        }


        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        //from ISaveable
        public object CaptureState()
        {
            return currentWeaponConfig.name; // loads name as a string into CaptureState()
        }
        public void RestoreState(object state) 
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

    }
}
