using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;


    void Update()
    {
        //point main camera to mouse pos when left mouse is clicked
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; //out stores info to hit variable
        //create a racast that takes in a ray, an out, which outputs hit info, and a hit.. as a bool
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;//formerly target.position;
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        //simplify into local (only velocity) to be useful for the animator
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        //set forwardSpeed value to value of speed
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

}

    
