using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAndBow : MonoBehaviour
{
    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivateTimer = 3f;

    public float damage = 50f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    void DeactivateGameObject()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;

        transform.LookAt(transform.position + myBody.velocity);
    }

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == Tags.ENEMY_TAG)
        {
            target.GetComponent<Health>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }    
    }
}
