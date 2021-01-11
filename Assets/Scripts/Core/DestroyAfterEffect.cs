using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) //IsAlive is a unity particle bool
            {
                Destroy(gameObject);
            }
        }
    }
}