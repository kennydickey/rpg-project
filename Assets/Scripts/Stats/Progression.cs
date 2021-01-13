using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        //our actual field
        [SerializeField] ProgressionCharacterClass[] characterClasses = null; //null starts it as 0

        [System.Serializable] // unity had not recognized as such, so we specify
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass; //our enum script to select roles
            [SerializeField] float[] health; //an array of floats with a selectable size
        }

    }
}