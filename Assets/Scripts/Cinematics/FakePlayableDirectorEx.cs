using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//example used in conjunction with CinematicsControlRemover, lecture 66
namespace RPG.Cinematics
{
    public class FakePlayableDirector : MonoBehaviour
    {
        public event Action<float> onFinish; //var that represents a list of callbacks

        void Start()
        {
            Invoke("OnFinish", 3f); //invoke this method after 3 seconds
        }

        void OnFinish()
        {
            onFinish(4.3f); ////4.3f is just a placeholder
        }
    }
}
