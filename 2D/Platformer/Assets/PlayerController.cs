using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Animations;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float jumpForce = 5f;
    [SerializeField]
    BoxCollider2D collide;
    [SerializeField]
    float climbingSpeed = 5f;
    [SerializeField]
    BoxCollider2D bodyCollider;
    [SerializeField]
    Vector2 hitKick = new Vector2(50f, 50f);
    [SerializeField]
    Transform hurtbox;
    [SerializeField]
    float attackRadius = 3f;
    [SerializeField] AudioClip JumpSFX, AttackSFX, getHitSFX, walkingSFX;

    [SerializeField]
    float startingGravity;
    
    Animator anim;

    bool isHurt = false;

    AudioSource myAudioSource;

    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;
        myAudioSource = GetComponent<AudioSource>();

        anim.SetTrigger("Apearing");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHurt)
        {
            Run();
            Jump();
            Climb();
            Attack();
            ExitLevel();

            if (collide.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit();
            }
        }
        
    }

    private void ExitLevel()
    {
        if (!collide.IsTouchingLayers(LayerMask.GetMask("Interactable")))
        {
            return;
        }
        if(CrossPlatformInputManager.GetButtonDown("Vertical"))
        {
            anim.SetTrigger("Entering");
            
        }
    }

    public void LoadNextLevel()
    {
        FindObjectOfType<Door>().StartLoadingNextLevel();
        TurnOffRenderer();
    }


    public void TurnOffRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }    

    private void Attack()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("isAttacking");
            myAudioSource.PlayOneShot(AttackSFX);
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(hurtbox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach(Collider2D enemy in enemiesToHit)
            {
                enemy.GetComponent<EnemyScript>().Die();
            }
        }

    }

    public void PlayerHit()
    {
        rb.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);

        anim.SetTrigger("isHit");
        myAudioSource.PlayOneShot(getHitSFX);

        isHurt = true;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
        StartCoroutine(StopHurting());
    }

    IEnumerator StopHurting()
    {
        yield return new WaitForSeconds(2f);

        isHurt = false;
    }

    private void Climb()
    {
        if(bodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            anim.SetBool("isClimbing", true);
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(rb.velocity.x, controlThrow * climbingSpeed);

            rb.velocity = climbingVelocity;
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = startingGravity;
            anim.SetBool("isClimbing", false);
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelocity = new Vector2(controlThrow * speed, rb.velocity.y);
        rb.velocity = playerVelocity;
        FlipSprite();
        ChangeAnimationRun();
    }

    void StepsSFX()
    {
        bool playerMovingHorizontally = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if(playerMovingHorizontally)
        {
            if(collide.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myAudioSource.PlayOneShot(walkingSFX);
            }
        }
        else
        {
            myAudioSource.Stop();
        }
    }

    private void Jump()
    {
        if(collide.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
            if (isJumping)
            {
                Vector2 playerJump = new Vector2(rb.velocity.x, jumpForce);
                rb.velocity = playerJump;
                myAudioSource.PlayOneShot(JumpSFX);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtbox.position, attackRadius);
    }
    private void ChangeAnimationRun()
    {
        bool runningHorizontal = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", runningHorizontal);
    }

    private void FlipSprite()
    {
        bool runningHorizontal = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if(runningHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

}
