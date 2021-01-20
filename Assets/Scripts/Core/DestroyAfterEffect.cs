using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        // drag in parent obj of vfx, that is what we will destroy
        [SerializeField] GameObject targetToDestroy = null;

        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) //IsAlive is a unity particle bool
            {
                if(targetToDestroy != null) // if target exists
                {
                    Destroy(targetToDestroy); // destroy obj we specified in SerializeField
                }
                else
                {
                    Destroy(gameObject); // destroy gameObject on which this script is attached
                }
            }
        }
    }
}