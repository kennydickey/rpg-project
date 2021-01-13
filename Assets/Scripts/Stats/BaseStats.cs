using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] // slider
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass; // so named enum of type CharacterClass
        [SerializeField] Progression progression = null;

        //called from progression
        public float GetHealth()
        {
            return progression.GetHealth(characterClass, startingLevel);
        }
    }
}
