using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 20f;

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

        // all CapturedState() content must be marked as Serializable
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
            //account for rotation update vv delete above ^^
            //// Dictionary<key, val> called data
            ////We can sore any obj inside this dictionary
            //Dictionary<string, object> data = new Dictionary<string, object>();
            //data["position"] = new SerializableVector3(transform.position);
            ////vector representation of rotation v                   v
            //data["rotation"] = new SerializableVector3(transform.eulerAngles);
            //return data;
        }
        // called just after Awake() and before Start()
        public void RestoreState(object state) // things in CapturedState will be restored
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            //account for rotation update vv delete above ^^
            //Dictionary<string, object> data = (Dictionary<string, object>)state;
            //GetComponent<NavMeshAgent>().enabled = false; //keeps NavMesh from disrupting our pos
            //// looks in our data objext for key of "position"
            //transform.position = ((SerializableVector3)data["position"]).ToVector(); //converts a serialized Vector3 to a unity readable Vector3
            //transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            //GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

    
