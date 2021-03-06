﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        //[SerializeField] GameObject persistentObjectPrefab;
        [SerializeField] GameObject persistentObjectPrefab = null;


        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            // GameObject is not actually changing so this is ok in Awake()
            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}