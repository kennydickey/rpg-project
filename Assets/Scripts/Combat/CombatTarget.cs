using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))] //auto places health component on obj when placing this class
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) //if cannot attack bool
            {
                return false;
            }
            if (Input.GetMouseButton(0)) //getMouseButtonDown(0) for click type interface
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            
            return true; //InteractWithCombat is now true
        }
    }

}