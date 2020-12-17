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

        NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        //method for use in PlayerController
        public void StartMoveAction(Vector3 destination)//like MoveTo, but only once when we click
        {
            //implementation of IAction
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination; //hit.point;
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

    
