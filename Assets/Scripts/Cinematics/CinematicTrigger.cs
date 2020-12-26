using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //includes Playable Director in inspector

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alReadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!alReadyTriggered && other.gameObject.tag == "Player")
            {
                alReadyTriggered = true;
                GetComponent<PlayableDirector>().Play();

            }
        }
    }
}

