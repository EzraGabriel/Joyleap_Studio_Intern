using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed, bulletLife;

    public Rigidbody myRb;

    public ParticleSystem explosionEffect;
    public bool rocketLauncher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(bulletLife);
        BulletFly();

        bulletLife -= Time.deltaTime;

        if (bulletLife < 0)
        {
            Destroy(gameObject);
        }
    }

    private void BulletFly()
    {
        myRb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rocketLauncher)
        {
            Instantiate(explosionEffect,transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
