using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObject : MonoBehaviour
    {
        [SerializeField] GameObject persistenObjectPrefab;

        static bool hasSpawned = false;
        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentobject = Instantiate(persistenObjectPrefab);
            DontDestroyOnLoad(persistentobject);
        }
    }
}