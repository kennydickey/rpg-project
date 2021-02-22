using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public interface ISaveable
    {

    }
}

//namespace RPG.Saving
//{
//    // basically just an interface that we can implement in another script, out methods will populate when we implement
//    public interface ISaveable
//    {
//        object CaptureState(); // gets state from whatever - movement, health etc
//        void RestoreState(object state); // now, whatever can update itself
//    }
//}