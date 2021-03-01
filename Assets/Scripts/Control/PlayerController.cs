using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
            //
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.none);
                return;
            }
            if (InteractWithComponent()) return;
            //call IWCombat t/f and skip over movement if true
            if (InteractWithMovement()) return; //remember.. return also exits the method

            //when not interacting with above..
            SetCursor(CursorType.none);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted(); //accountant raycast
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            //sorting the hits based on distances
            Array.Sort(distances, hits); //sorting array by keys and items, items arranged based on keys
            return hits;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject()) //returns a bool
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;

        }

        private bool InteractWithMovement()
        {          

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit) //if able to hit the ground..
            {
                //point main camera to mouse pos when left mouse is clicked
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f); //formerly target.position;
                }
                SetCursor(CursorType.movement);
                return true; //return also exit method
            }
            return false; //'else' return false
        }

        private bool RaycastNavMesh(out Vector3 target) //out gives us location to move to
        {
            target = new Vector3();
            RaycastHit hit; //out stores info to hit variable
                            //create a racast that takes in a ray, an out, which outputs hit info, and a hit.. as a bool
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            //find nearest navmesh point
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;
            // return true if found            
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
