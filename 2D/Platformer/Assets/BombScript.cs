using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    Animator anim;
    [SerializeField] float radius = 3f;
    [SerializeField] Vector2 explosionForce = new Vector2(200f, 100f);
    [SerializeField] AudioClip explodeSFX, burnSFX;

    AudioSource myAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    void ExplodeBomb()
    {
        Collider2D playerCollider =  Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        myAudioSource.PlayOneShot(explodeSFX);
        if(playerCollider)
        {
            playerCollider.GetComponent<Rigidbody2D>().AddForce(explosionForce);

            playerCollider.GetComponent<PlayerController>().PlayerHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Burn");
        myAudioSource.PlayOneShot(burnSFX);
    }

    void DestroyBomb()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
