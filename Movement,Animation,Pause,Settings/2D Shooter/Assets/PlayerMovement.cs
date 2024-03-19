using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    //public Animator anim;
    public BoxCollider2D groundCheck;

    public float runSpeed = 40f;

    float horizontalMove = 0;
    bool jump = false;
    bool crouch = false;


    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //anim.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            //anim.SetBool("isJumping", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            crouch = true;
            //head.enabled = false;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            crouch = false;
            //head.enabled = true;
        }
    }


    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump) ;
        jump = false;
    }


    public void OnCrouching(bool isCrouching)
    {
        //anim.SetBool("isCrouching", isCrouching);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //anim.SetBool("isJumping", false);
    }

}
