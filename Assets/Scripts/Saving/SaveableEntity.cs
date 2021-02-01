using System.Collections;
using System.Collections.Generic;
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

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return; //
            print("editing");
        }

    }
}