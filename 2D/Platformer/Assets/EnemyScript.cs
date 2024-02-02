using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float enemySpeed = 5f;
    [SerializeField] AudioClip dyingSFX;
    Animator enemyAnim;
    Rigidbody2D enemyRb;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();

    }

    public void Die()
    {
        enemyAnim.SetTrigger("isDead");
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        enemyRb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(Gone());
    }

    private void EnemyMovement()
    {
        if (IsFacingLeft())
        {
            enemyRb.velocity = new Vector2(-enemySpeed, 0f);
        }
        else
        {
            enemyRb.velocity = new Vector2(enemySpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipSprites();
    }

    private void FlipSprites()
    {
        transform.localScale = new Vector2(Mathf.Sign(enemyRb.velocity.x), 1f);
    }

    private bool IsFacingLeft()
    {
        return transform.localScale.x > 0;
    }

    IEnumerator Gone()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void PlayDyingSFX()
    {
        AudioSource.PlayClipAtPoint(dyingSFX, Camera.main.transform.position);
    }
}
