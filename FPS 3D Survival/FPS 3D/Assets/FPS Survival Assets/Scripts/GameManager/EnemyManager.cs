using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boarPrefab, cannibalPrefab;

    public Transform[] cannibalSpawnpoints, boarSpawnpoints;

    [SerializeField]
    private int cannibalEnemyCount, boarEnemyCount;
    private int initialCannibalCount, initialBoarCount;

    public float waitBeforeSpawnTime = 10f;
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        initialBoarCount = boarEnemyCount;
        initialCannibalCount = cannibalEnemyCount;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }
    void SpawnCannibals()
    {
        int index = 0;
        for (int i = 0; i < cannibalEnemyCount; i++)
        {
            if (index >= cannibalSpawnpoints.Length)
            {
                index = 0;
            }
            Instantiate(cannibalPrefab, cannibalSpawnpoints[index].position, Quaternion.identity);
            index++;
        }
        cannibalEnemyCount = 0;
    }
    void SpawnBoars()
    {
        int index = 0;
        for (int i = 0; i < boarEnemyCount; i++)
        {
            if (index >= boarSpawnpoints.Length)
            {
                index = 0;
            }
            Instantiate(boarPrefab, boarSpawnpoints[index].position, Quaternion.identity);
            index++;
        }
        boarEnemyCount = 0;
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(waitBeforeSpawnTime);

        SpawnCannibals();
        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool cannibal)
    {
        if(cannibal)
        {
            cannibalEnemyCount++;
            if(cannibalEnemyCount > initialCannibalCount)
            {
                cannibalEnemyCount = initialCannibalCount;
            }
        }
        else
        {
            boarEnemyCount++;
            if(boarEnemyCount > initialBoarCount)
            {
                boarEnemyCount = initialBoarCount;
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }
}
