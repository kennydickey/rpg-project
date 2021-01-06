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
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform; //var declaration of type transform
                if (isRightHanded) handTransform = rightHand;
                else handTransform = leftHand;
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