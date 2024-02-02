using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float stopTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 10f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        float timeSinceLastSaw = Mathf.Infinity;
        float timeSinceLastCheckpoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWayPointIndex = 0;

        LazyValue<Vector3> guardLocation;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();
            guardLocation = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardLocation.value = transform.position;
        }
        
        private void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                
                AttackBehaviour();
            }
            else if (timeSinceLastSaw < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimeMethod();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimeMethod()
        {
            timeSinceLastSaw += Time.deltaTime;
            timeSinceLastCheckpoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation.value;
            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    timeSinceLastCheckpoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceLastCheckpoint > stopTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSaw = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits =  Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if(ai == null)
                {
                    continue;
                }
                ai.Aggrevate();            }
        }

        private bool IsAggrevated()
        {
            float v = Vector3.Distance(player.transform.position, transform.position);
            return v < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}