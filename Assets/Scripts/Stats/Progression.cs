using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        //our actual field
        [SerializeField] ProgressionCharacterClass[] characterClasses = null; //null starts it as 0

        public float GetStatProg(Stat stat, CharacterClass characterClass, int level)
        {
            // iterate over characterClasses Array
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != characterClass) continue; // next iteration

                // iterate over serialized stats
                foreach(ProgressionStat progressionStat in progressionClass.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    if (progressionStat.levels.Length < level) continue; // if lvl is not in array
                    // otherwise..
                    return progressionStat.levels[level - 1];
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