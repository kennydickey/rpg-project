using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier 
        {
            A, B , C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination; //enum dropdown menu

        private void OnTriggerEnter(Collider other) //any other collider
        {
            if (other.tag == "Player")
            {
                //SceneManager.LoadScene(sceneToLoad);
                StartCoroutine(Transition());
            }
        
        }

        private IEnumerator Transition()
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break; // rather than return null for IEnum
            }

            DontDestroyOnLoad(gameObject); // keeps portal gameObject, assuming portal is at root of scene
            //IEnums require a yield return
            //yield and call again when scene is finished loading
            yield return SceneManager.LoadSceneAsync(sceneToLoad); //allows us to transfer info into new scene
            //after scene load

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject); // this script is attached to portal, so will destroy portal
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            // player.GetComponent<NavMeshAgent>().enabled = false; // keep navmesh from placing player
            // alternatively, simply use NavMesh to place player using the same destination
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue; //go to next obj in loop
                if (portal.destination == this.destination) continue;
                // otherwise..
                return portal;
            }
            // if none are found
            return null;
        }
    }
}