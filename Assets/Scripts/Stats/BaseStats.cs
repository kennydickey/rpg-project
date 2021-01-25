using System;
using GameDevTV.Utils;
using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {

        [Range(1, 99)] // slider
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Player; // so named enum of type CharacterClass
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        //check box v
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() // to be ready for other methods to call before Start()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel); //get initial level, CalculateLevel() accesses another class, so cannot move to Awake(). The other class will not have initialized yet, ! update, we place method in awake to initialize without being called right away
        }

        private void Start()
        {
            currentLevel.ForceInit(); 
        }

        private void OnEnable() // called around the same time as Awake()s are happening, but always after Awake() for the same method. Cannot use external functions as states are not yet set
        {
            if (experience != null)
            {
                //subscribing to onExperienceGained Action Delegate and adding to it
                experience.onExperienceGained += UpdateLevel; //added UpdateLevel method to call list
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                //removing subscription to onExperienceGained Action Delegate when Disable()d, so that it doesn't continue to get notifications from OnExperienceGained
                experience.onExperienceGained -= UpdateLevel; //removed UpdateLevel method from call list
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel(); // within update, so checks for new level change
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel; // update current level
                LevelUpEffect();
                onLevelUp(); // delegate method calling al things subscribed to onLevelUp
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform); // instantiate this obj at parent location
        }

        //called from Health to get various stats
        public float GetStat(Stat stat)
        {
            // return the actual value in the scriptable object Progression
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifiers(stat)/100);
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStatProg(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0; // exit method for enemies who don't have modifiers

            float total = 0;
            // for each one in multiple Imods           notice v
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                // for each stat within each provider..
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier; // add each modifier stat's value to total
                }
            }
            return total;
        }


        private float GetPercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0; // exit method for enemies who don't have modifiers

            float total = 0;
            // for each one in multiple Imods           notice v
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                // for each stat within each provider..
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier; // add each modifier stat's value to total
                }
            }
            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            // for enemy non experience
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            // penultimate is the length of the array returned from GetLevels()
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            // Iterate over all levels fro 1 to penultimate
            for (int level = 1; level <= penultimateLevel; level++)
            {
                // xp to level up for each particular level
                float XPToLevelUp = progression.GetStatProg(Stat.ExperienceToLevelUp, characterClass, level);
                // if we have less than current, output current level 
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }



            return penultimateLevel + 1;
        }

    }
}
