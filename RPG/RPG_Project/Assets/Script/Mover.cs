﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 15f;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        private void Awake()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2)
            {
                return total;
            }
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath)
            {
                return false;
            }
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            if (GetPathLength(path) > maxNavPathLength)
            {
                return false;
            }
            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localvelocity = transform.InverseTransformDirection(velocity);
            float speed = localvelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }


        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

}