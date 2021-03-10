using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        // for accurate visual representation, this should update after other things that may modify the transform
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}