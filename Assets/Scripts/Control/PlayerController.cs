using UnityEngine;
using System.Collections;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        private void Update()
        {
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            //type RaycastHit array of hits, takes in obj's from RaycastAll
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); //accountant raycast
            foreach (RaycastHit hit in hits)
            {
                //for each obj in hit location, get this component
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; //not a combatTarget. go on to next item in loop
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
            }
        }

        private void InteractWithMovement()
        {
            //point main camera to mouse pos when left mouse is clicked
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            RaycastHit hit; //out stores info to hit variable
                            //create a racast that takes in a ray, an out, which outputs hit info, and a hit.. as a bool
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                GetComponent<Mover>().MoveTo(hit.point); //formerly target.position;
            }
        }

        private static Ray GetMouseRay()
        {
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
