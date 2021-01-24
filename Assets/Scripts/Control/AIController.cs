using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f; //5 unity units
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f; //1 meter?
        [SerializeField] float waypointDwellTime = 2f;
        [Range(0,1)] // range of field below v
        [SerializeField] float patrolSpeedFraction = 0.2f; //fractin of max speed

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity; //initially very high
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; //has not arrived yet
        int currentWaypointIndex = 0;

        private void Awake()
        {
            // references below are set up in Awake() to be set up for any public methods that are called in Start() elsewhere
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            // ! should not access transform on Awake() like our GetComponents
            guardPosition = transform.position; //placed in start() to be a fixed return pos
        }


        private void Update() //AI states will be here
        {
            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) // if xfloat < yfloat
            {
                // timeSinceLastSawPlayer = 0; // resets time since, moved into AttackBehaviour
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //suspicion state
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }


        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime; // time in seconds since the last frame
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0; //when at waypoint, start timer
                    CycleWaypoint();
                }
                //otherwise..
                nextPosition = GetCurrentWaypoint();
           
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction); //(guardPosition)
            }         
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance; //bool
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0; //resets time
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer() //returns a float
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance; //returns a bool
        }

        //called by unity when drawing gizmos, similar to update and start
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}