using UnityEngine;

namespace RPG.Combat
{
    // Menu creation! In Unity, right click Create / Weapons / MAke New Weapon
    [CreateAssetMenu(fileName = "weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        // Below is anything that will change based on weapon wielded
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject weaponPrefab = null; //unequipped at first

        public void Spawn(Transform handTransform, Animator animator)
        {
            Instantiate(weaponPrefab, handTransform); // object, location
            animator.runtimeAnimatorController = animatorOverride;
        }


    }
}