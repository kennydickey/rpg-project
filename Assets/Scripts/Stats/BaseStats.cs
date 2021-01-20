using System;
using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {

        [Range(1, 99)] // slider
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass; // so named enum of type CharacterClass
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        int currentLevel = 0; //initialize current level

        public event Action onLevelUp;

        private void Start()
        {
            currentLevel = CalculateLevel(); //get initial level
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                //subscribing to onExperienceGained Action Delegate and adding to it
                experience.onExperienceGained += UpdateLevel; //added UpdateLevel method to call list
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel(); // within update, so checks for new level change
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel; // update current level
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
            return progression.GetStatProg(stat, characterClass, GetLevel()); 
        }

        public int GetLevel()
        {
            if(currentLevel < 1) // if currentLevel is not initialized with 1+
            {
                currentLevel = CalculateLevel(); // makes sure we have currentLevel set for next call
            }
            return currentLevel;
        }

        public int CalculateLevel()
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
