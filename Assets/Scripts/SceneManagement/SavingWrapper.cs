﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 2f;

        // Start can return an IEnumerator, which auto runs because start is it's own coroutine
        IEnumerator Start()
        {
            //    Fader fader = FindObjectOfType<Fader>();
            //    fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        //    yield return fader.FadeIn(fadeInTime);
        }

    void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }
        public void Load()
        {
            //call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Save()
        {
            //call to saving system Save
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}

