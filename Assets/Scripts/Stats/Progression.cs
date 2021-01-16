using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        //our actual field
        [SerializeField] ProgressionCharacterClass[] characterClasses = null; //null starts it as 0

        //              key                val<key, val>
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStatProg(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            //return lookupTable[characterClass][stat][level]; //rather..
            float[] levels = lookupTable[characterClass][stat];

            if(levels.Length < level) //if level is higher than array
            {
                return 0;
            }
            //otherwise..
            return levels[level - 1];

            //// iterate over characterClasses Array
            //foreach(ProgressionCharacterClass progressionClass in characterClasses)
            //{
            //    if (progressionClass.characterClass != characterClass) continue; // next iteration

            //    // iterate over serialized stats
            //    foreach(ProgressionStat progressionStat in progressionClass.stats)
            //    {
            //        if (progressionStat.stat != stat) continue;
            //        if (progressionStat.levels.Length < level) continue; // if lvl is not in array
            //        // otherwise..
            //        return progressionStat.levels[level - 1];
            //    }
            //}
            //// otherwise default to vv
            //return 0;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return; // if lookupTable was already built
            //create outer Dictionary // () is (empty)
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();


            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                //populate statLookupTable by iterating to insert keys / vars
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    // inner Dictionary             key                     var
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                // outer dictionary          key               var
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
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