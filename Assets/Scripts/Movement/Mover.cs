using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    // interfaces we want to derive from
    public class Mover : MonoBehaviour, IAction//, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 20f;
        [SerializeField] float maxNavPathLength = 40f;


        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            // used in Awake so that RestoreState or others can use them before Start()
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

        public bool CanMoveTo(Vector3 destination)
        {
            //cannot move to location if no path
            NavMeshPath path = new NavMeshPath(); // needs an obj like NavMeshPath, so CalculatePath can modify
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            // return true if found
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
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


        private float GetPathLength(NavMeshPath path)
        {
            float total = 0; //accumulator?
            if (path.corners.Length < 2) return total; //corners are points along the navmesh path
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;

        }

        //public object CaptureState()
        //{
        //    // transform.position on it's own is not serializeable, so..
        //    return new SerializableVector3(transform.position);
        //}

        //public void RestoreState(object state)
        //{
        //    //print("restoring state for " + GetUniqueIdentifier()); // to test
        //    SerializableVector3 position = (SerializableVector3)state; // load our serialized position data as position
        //    GetComponent<NavMeshAgent>().enabled = false; // to prevent movement glitches when Navmesh is also trying to move things
        //    transform.position = position.ToVector(); // update our position as a Vector3
        //    GetComponent<NavMeshAgent>().enabled = true;
        //    GetComponent<ActionScheduler>().CancelCurrentAction(); // cancel action when moving to a point
        //}

        // all CapturedState() content must be marked as Serializable
        //public object CaptureState()
        //{
        //    return new SerializableVector3(transform.position);
        //    //account for rotation update vv delete above ^^
        //    //// Dictionary<key, val> called data
        //    ////We can sore any obj inside this dictionary
        //    //Dictionary<string, object> data = new Dictionary<string, object>();
        //    //data["position"] = new SerializableVector3(transform.position);
        //    ////vector representation of rotation v                   v
        //    //data["rotation"] = new SerializableVector3(transform.eulerAngles);
        //    //return data;
        //}
        // always need to be called just after Awake() and before Start()
        //public void RestoreState(object state) // things in CapturedState will be restored
        //{
        //    SerializableVector3 position = (SerializableVector3)state;
        //    navMeshAgent.enabled = false; // navMeshAgent declared in awake()
        //    transform.position = position.ToVector();
        //    navMeshAgent.enabled = true;
        //    GetComponent<ActionScheduler>().CancelCurrentAction();
        //    //account for rotation update vv delete above ^^
        //    //Dictionary<string, object> data = (Dictionary<string, object>)state;
        //    //GetComponent<NavMeshAgent>().enabled = false; //keeps NavMesh from disrupting our pos
        //    //// looks in our data objext for key of "position"
        //    //transform.position = ((SerializableVector3)data["position"]).ToVector(); //converts a serialized Vector3 to a unity readable Vector3
        //    //transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
        //    //GetComponent<NavMeshAgent>().enabled = true;
        //}
    }
}

    
