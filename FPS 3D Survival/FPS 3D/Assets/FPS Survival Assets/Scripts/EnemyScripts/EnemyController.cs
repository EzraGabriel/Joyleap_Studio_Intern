using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,CHASE,ATTACK
}

public class EnemyController : MonoBehaviour
{

    private EnemyAnimation enemyAnimation;
    private NavMeshAgent navMeshAgent;

    private EnemyState enemyState;

    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;

    public float chaseDistance = 7f;
    private float currentChaseDistance;
    public float attackDistance = 1.8f;
    public float chaseAfterAttackDistance = 2f;

    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    public float patrolForThisTime = 15f;
    private float patrolTimer;

    public float waitBeforeAttack = 2f;
    private float attackTimer;

    private Transform target;

    public GameObject attackPoint;

    private EnemyAudio enemyAudio;

    void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = Tags.ENEMY_TAG;
        enemyState = EnemyState.PATROL;

        patrolTimer = patrolForThisTime;

        attackTimer = waitBeforeAttack;

        currentChaseDistance = chaseDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }
        else if(enemyState == EnemyState.ATTACK)
        {
            Attack();
        }
        else if(enemyState == EnemyState.CHASE)
        {
            Chase();
        }
    }

    void Patrol()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSpeed;

        patrolTimer += Time.deltaTime;

        if(patrolTimer > patrolForThisTime)
        {
            SetNewRandomDestination();
            patrolTimer = 0f;
        }
        if(navMeshAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnimation.Walk(true);
        }
        else
        {
            enemyAnimation.Walk(false);
        }

        if(Vector3.Distance(transform.position, target.position)<= chaseDistance)
            {
            enemyAnimation.Walk(false);
            enemyState = EnemyState.CHASE;
            enemyAudio.Play_ScreamSound();
        }
    }

    private void SetNewRandomDestination()
    {
        float randRadius = UnityEngine.Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randDir = UnityEngine.Random.insideUnitSphere * randRadius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);

        navMeshAgent.SetDestination(navHit.position);
    }

    void Attack()
    {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;

        attackTimer += Time.deltaTime;

        if(attackTimer > waitBeforeAttack)
        {
            enemyAnimation.Attack();
            attackTimer = 0f;
            enemyAudio.PlayAttackSound();
        }

        if(Vector3.Distance(transform.position, target.position)> attackDistance + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.CHASE;
        }
    }

    void Chase()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = runSpeed;

        navMeshAgent.SetDestination(target.position);

        if (navMeshAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnimation.Run(true);
        }
        else
        {
            enemyAnimation.Run(false);
        }
        if(Vector3.Distance(transform.position, target.position)<= attackDistance)
        {
            enemyAnimation.Run(false);
            enemyAnimation.Walk(false);
            enemyState = EnemyState.ATTACK;

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
            else if (Vector3.Distance(transform.position, target.position) > chaseDistance)
            {
                enemyAnimation.Run(false);

                enemyState = EnemyState.PATROL;
                patrolTimer = patrolForThisTime;

                if(chaseDistance != currentChaseDistance)
                {
                    chaseDistance = currentChaseDistance;
                }
            }
        }
    }

    void Turn_On_AttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }

    }

    public EnemyState Enemy_State
    {
        get; set;
    }
}
