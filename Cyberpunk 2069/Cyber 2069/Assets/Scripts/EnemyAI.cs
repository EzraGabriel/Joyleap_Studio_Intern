using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent myAgent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform player;
    Animator anim;

    public Transform firePosition;

    public Vector3 destinationPoint;
    bool destinationSet;
    public float destinationRange;

    public float chaseRange;
    private bool playerInChaseRange;

    //Attacking
    public float attackRange, attackTime;
    private bool playerInAttackRange, readyToAttack = true;
    public GameObject attackingProjectile;

    //Melee
    public bool meleeAttacker;
    public int meleeDamageAmount = 2;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInChaseRange = Physics.CheckSphere(transform.position, chaseRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInChaseRange && !playerInAttackRange)
        {
            Guarding();
        }
        else if (playerInChaseRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInChaseRange && playerInAttackRange)
        {
            AttackPlayer();
        }

    }


    private void Guarding()
    {
        if (!destinationSet)
        {
            SearchForDestination();
        }
        else
        {
            myAgent.SetDestination(destinationPoint);
        }
        Vector3 distanceToDestination = transform.position - destinationPoint;

        if (distanceToDestination.magnitude < 1f)
        {
            destinationSet = false;
        }
    }
    private void ChasePlayer()
    {
        myAgent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        myAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (readyToAttack && !meleeAttacker)
        {
            anim.SetTrigger("isAttacking");

            firePosition.LookAt(player);
            Instantiate(attackingProjectile, firePosition.position, firePosition.rotation);

            readyToAttack = false;
            StartCoroutine(ResetAttack());
        }
        else if (readyToAttack && meleeAttacker)
        {
            anim.SetTrigger("isAttacking");
        }
    }

    public void MeleeDamage()
    {
        if (playerInAttackRange)
        {
            player.GetComponent<PlayerHealthSystem>().TakeDamage(meleeDamageAmount);
        }
    }

    private void SearchForDestination()
    {
        float randPositionZ = Random.Range(-destinationRange, destinationRange);
        float randPositionX = Random.Range(-destinationRange, destinationRange);


        destinationPoint = new Vector3(transform.position.x + randPositionX, transform.position.y, transform.position.z + randPositionZ);

        if (Physics.Raycast(destinationPoint, -transform.up, 2f, whatIsGround))
        {
            destinationSet = true;
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackTime);

        readyToAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
