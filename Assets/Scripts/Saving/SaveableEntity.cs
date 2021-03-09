using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

//namespace RPG.Saving
//{
//    [ExecuteAlways]
//    public class SaveableEntity : MonoBehaviour
//    {

//    }
//}

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        //simple uid, but unreliable and we need for each character
        //[SerializeField] string uniqueIdentifier = System.Guid.NewGuid().ToString();
        [SerializeField] string uniqueIdentifier = "";
        // statics live through the lifetime of the running application, even between scenes
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            //return "" + " got from getuniqueidentifier";
            return uniqueIdentifier;
        }

        public object CaptureState() //returns a serializeable object
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            // ISaveable contains a collection of components that implement the ISaveable interface
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                // update or create a key for each saveable, so "Mover" for example
                state[saveable.GetType().ToString()] = saveable.CaptureState(); // CaptureState stores "Mover" or whichever "Component" into dictionary ny string
            }

            return state; // state is the dictionary we created
            ////print("capturing state for " + GetUniqueIdentifier()); // to test

        }

        public void RestoreState(object state)
        {   //stateDict is state cast as this dictionary type bc we need it to be of this type
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR // <- only need this codeblock for editor, code will be ignored otherwise for packaging
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return; // exit method in play mode
            //print("path " + gameObject.scene.path); // for testing whether we are in a prefab, empty path is a prefab
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // exit method rather than assigning Guid

            // serializedObject is the serialization of this, which is our monobehaviour as a whole
            SerializedObject serializedObject = new SerializedObject(this);
            // narrow down to specific property and store as SerializedProperty property
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue)) // if empty
            {
                property.stringValue = System.Guid.NewGuid().ToString(); // generate new uid
                serializedObject.ApplyModifiedProperties(); // tells unity of the changes, which updates the sandbox scene so that we can apply to save file so that the correct state will be restored 
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            // check key exists in dictionary
            if (!globalLookup.ContainsKey(candidate)) return true; // ContainsKey returns true if not candidate

            // not pointing to ourselves
            if (globalLookup[candidate] == this) return true; // 'this' is current gameObject, so already unique

            if (globalLookup[candidate] == null) // we know it is unique because it was already null
            {
                globalLookup.Remove(candidate);
                return true;
            }
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }

    }
}