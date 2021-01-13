using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        //our actual field
        [SerializeField] ProgressionCharacterClass[] characterClasses = null; //null starts it as 0

        public float GetHealth(CharacterClass characterClass, int level)
        {
            // iterate over characterClasses Array
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if(progressionClass.characterClass == characterClass)
                {
                    return progressionClass.health[level - 1];
                }
            }
            // otherwise default to vv
            return 0;
        }

        [System.Serializable] // unity had not recognized as such, so we specify
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass; //our enum script to select roles
            public float[] health; //an array of floats with a selectable size
        }

    }
}