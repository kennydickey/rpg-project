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

        private void Awake()
        {
            StartCoroutine(LoadLastScene()); // immediately load scene on awake
        }

        //Start can return an IEnumerator, which auto runs because start is it's own coroutine
        //IEnumerator Start() //rather..
        IEnumerator LoadLastScene()
        {
            //yield return null;
            // broken here v v cannot go back to previous scene // fix - fader after LoadLastScene()
            // broken here now vv
            //yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            // coroutine will pause, allowing Awake()s, including fader's Awake() to happen before fader comes in
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
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
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
            print("save file deleted");
        }
    }
}

