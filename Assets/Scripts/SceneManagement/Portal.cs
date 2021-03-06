﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManageMent
{

}

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        //[SerializeField] Transform spawnPoint;
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] DestinationIdentifier destination;// = DestinationIdentifier.A; //enum dropdown menu
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) //any other collider
        {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(sceneToLoad);
                StartCoroutine(Transition());
            }

        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break; // rather than return null for IEnum
            }


            DontDestroyOnLoad(gameObject); // keeps portal gameObject, assuming portal is at root of scene

            Fader fader = FindObjectOfType<Fader>();

            // Save current level
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime); //yields take turns within the method each frame
            //yield and call again when scene is finished loading

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad); //allows us to transfer info into new scene

            // Load current level
            savingWrapper.Load();

            //after scene load..
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save(); // save again after loading the players location so that we have correct state when returning to game, player should be at the protal spawn point

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject); // this script is attached to portal, so will destroy portal
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false; // keep navmesh from placing player
            // alternatively, simply use NavMesh to place player using the same destination
            //player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue; //go to next obj in loop
                if (portal.destination != destination) continue; //revert to// == this.destination //maybe
                // otherwise..
                return portal;
            }
            // if none are found
            return null;
        }
    }
}