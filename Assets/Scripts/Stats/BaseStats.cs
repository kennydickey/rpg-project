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

        //called from Health to get various stats
        public float GetStat(Stat stat)
        {
            // return the actual value in the scriptable object Progression
            return progression.GetStatProg(stat, characterClass, startingLevel); 
        }

        public int GetLevel()
        {
            GetComponent<Experience>();
        }

    }
}
