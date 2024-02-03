using UnityEngine;
using System.Collections;

public class MoveCar : MonoBehaviour {

    public float speed = 17;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.right * -speed;
	}
}
