using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {

        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                //Destroy(gameObject); // Gameobject gameObject of which this script is attached
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow; // !only for the collider next to this script
            //uncheck the actual weapon obj vv
            //transform.GetChild(0).gameObject.SetActive(shouldShow); //or all childed objs..
            foreach(Transform child in transform) // all objs in transform, child can be named anything
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

    }
}
