using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private void Update()
    {
            //point main camera to mouse pos when left mouse is clicked
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
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
            GetComponent<Mover>().MoveTo(hit.point); //formerly target.position;
        }
    }


}
