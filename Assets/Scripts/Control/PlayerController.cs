﻿using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            //call IWCombat t/f and skip over movement if true
            if (InteractWithCombat()) return; //remember.. return also exits the method
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() //finally not a void!
        {
            //type RaycastHit array of hits, takes in obj's from RaycastAll
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); //accountant raycast
            foreach (RaycastHit hit in hits)
            {
                //for each obj in hit location, get this component
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; //next item in foreach

                if (!GetComponent<Fighter>().PlayerCanAttack(target.gameObject)) //if cannot attack bool
                {
                    continue; //cannot attack, go on to next item in foreach
                }
                if (Input.GetMouseButton(0)) //getMouseButtonDown(0) for click type interface
                {               
                    GetComponent<Fighter>().Attack(target.gameObject);                  
                }
                return true; //InteractWithCombat is now true
            }
            //while raycasts in loop are not returning true..
            return false; //'else' return false
        }

        private bool InteractWithMovement()
        {        
            RaycastHit hit; //out stores info to hit variable
                            //create a racast that takes in a ray, an out, which outputs hit info, and a hit.. as a bool
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                //point main camera to mouse pos when left mouse is clicked
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f); //formerly target.position;
                }
                return true; //return also exit method
            }
            return false; //'else' return false
        }

        private static Ray GetMouseRay()
        {
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
