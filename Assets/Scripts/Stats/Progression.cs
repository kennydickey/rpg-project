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
                    //return progressionClass.health[level - 1];
                }
            }
            // otherwise default to vv
            return 0;
        }


        // enum tree for Progression scriptable object
        [System.Serializable] // unity had not recognized as such, so we specify
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass; //our enum script to select roles
            //public float[] health; //an array of floats with a selectable size
            public ProgressionStat[] stats; // slectable number of stats
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat; // enum selector for whether we are choosing health, exp, etc
            public float[] levels; // arr for level of each stat
        }

    }
}