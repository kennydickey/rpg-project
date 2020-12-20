using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f; //5 unity units

        private void Update()
        {
            if (DistanceToPlayer() < chaseDistance) // if xfloat < yfloat
            {              
                print(gameObject.name + "should chase");
            }
        }

        private float DistanceToPlayer() //returns a float
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}