using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public float duration = 4f;
    public int buff = 80;
    public Text powerup;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           StartCoroutine( Pickup(collision) );
            powerup.enabled = false;
        }
        
    }

     IEnumerator Pickup(Collider2D player)
    {
        WeaponRaycast rc = player.GetComponent<WeaponRaycast>();
        rc.damage = 120;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
            yield return new WaitForSeconds(duration);
        rc.damage = 40;
        Destroy(gameObject);
    }
}
