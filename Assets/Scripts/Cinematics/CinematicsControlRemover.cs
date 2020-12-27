using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;
using RPG.Core;

namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        GameObject player; // private var to cache the player in Start() for use in other methods

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; //notice no call parens
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }
        void DisableControl(PlayableDirector playabledirector)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
        void EnableControl(PlayableDirector playabledirector)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}