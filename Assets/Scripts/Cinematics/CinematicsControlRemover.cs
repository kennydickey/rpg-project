using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; //notice no call parens
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        void DisableControl(PlayableDirector playableDirector)
        {
            print("disablecontrol");
        }
        void EnableControl(PlayableDirector playableDirector)
        {
            print("enablecontrol");
        }
    }
}