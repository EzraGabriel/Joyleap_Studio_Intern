using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 move_Direction;
    public float speed = 5f;
    private float gravity = 20f;

    public float jumpForce = 10f;
    private float vertical_Velocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        MoveThePlayer(); 
    }

    void MoveThePlayer()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        characterController.Move(move_Direction);
    }

    private void ApplyGravity()
    {
        vertical_Velocity -= gravity * Time.deltaTime;

        PlayerJump();

        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }

    private void PlayerJump()
    {
        if(characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jumpForce;
        }
    }
}
