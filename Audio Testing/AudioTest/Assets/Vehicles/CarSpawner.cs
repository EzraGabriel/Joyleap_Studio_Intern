using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public float timeBetweenSpawns = 10;
    public GameObject CarPrefab;
    float lastSpawn = 0;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	    if (lastSpawn + timeBetweenSpawns < Time.time)
        {
            Instantiate(CarPrefab, transform.position, transform.rotation);
            lastSpawn = Time.time;
        }
	}
}
