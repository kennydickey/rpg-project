using UnityEngine;

namespace RPG.Combat
{
    //Scriptable Object!

    // Menu creation! In Unity, right click Create / Weapons / MAke New Weapon
    [CreateAssetMenu(fileName = "weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        // Below is anything that will change based on weapon wielded
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null; //unequipped at first
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform); // object, location
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetWeaponRange()
        {
            return weaponRange;

        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

    }
}