using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        //simple uid, but unreliable and we need for each character
        //[SerializeField] string uniqueIdentifier = System.Guid.NewGuid().ToString();
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return "" + " got from getuniqueidentifier";
        }

        public object CaptureState()
        {
            print("capturing state for " + GetUniqueIdentifier());
            return null;
        }

        public void RetoreState(object state)
        {
            print("restoring state for " + GetUniqueIdentifier());

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

            if(string.IsNullOrEmpty(property.stringValue)) // if empty
            {
                property.stringValue = System.Guid.NewGuid().ToString(); // generate new uid
                serializedObject.ApplyModifiedProperties(); // tells unity of the changes, which updates the sandbox scene so that we can apply to save file so that the correct state will be restored 
            }
        }
#endif
    }
}