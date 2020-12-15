using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        //update after anim to let anim do it's thing first to prevent jitter
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
