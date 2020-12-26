using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); //IsDead() is a bool, enabled() becomes false

            UpdateAnimator();
        }

        //method for use in PlayerController
        public void StartMoveAction(Vector3 destination, float speedFraction)//like MoveTo, but only once when we click
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            GetComponent<NavMeshAgent>().destination = destination; //hit.point;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); //Clamp01 means has to be val of 0 to 1
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }        
        
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            //simplify into local (only velocity) to be useful for the animator
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            //set forwardSpeed value to value of speed
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

    }
}

    
