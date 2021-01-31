using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{ 
    public class SaveableEntity : MonoBehaviour
    {
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

    }
}