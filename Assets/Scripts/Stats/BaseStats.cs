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

        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                print(GetLevel());
            }
        }

        //called from Health to get various stats
        public float GetStat(Stat stat)
        {
            // return the actual value in the scriptable object Progression
            return progression.GetStatProg(stat, characterClass, GetLevel()); 
        }

        public int GetLevel()
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
