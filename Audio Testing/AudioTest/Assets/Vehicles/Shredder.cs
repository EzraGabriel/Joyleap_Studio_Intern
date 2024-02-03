using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Car")
        {
            Destroy(other.gameObject);
        }
    }
}
