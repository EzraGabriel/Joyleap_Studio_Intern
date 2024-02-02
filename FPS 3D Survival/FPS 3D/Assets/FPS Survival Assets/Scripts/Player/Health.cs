﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    private EnemyAnimation enemyAnimation;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;

    public float health = 100f;
    public bool is_Player, is_Boar, is_Cannibal;
    private bool is_Dead;
    private EnemyAudio enemyAudio;
    private PlayerStats playerStats;

    void Awake()
    {
        if (is_Boar || is_Cannibal)
        {
            enemyAnimation = GetComponent<EnemyAnimation>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
        else if (is_Player)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }
    public void ApplyDamage(float damage)
    {
        if(is_Dead)
        {
            return;
        }
        health -= damage;

        if(is_Player)
        {
            playerStats.DisplayHealthStats(health);
        }

        if(is_Boar || is_Cannibal)
        {
            if(enemyController.Enemy_State == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }
        if(health<1)
        {
            PlayerDied();
            is_Dead = true;
        }
    }

    private void PlayerDied()
    {
        if(is_Cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);

            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnimation.enabled = false;
            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(true);
        }

        if(is_Boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemyController.enabled = false;
            enemyAnimation.Dead();
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(false);
        }
        if(is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }
            EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if(tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDeadSound();
    }
}
