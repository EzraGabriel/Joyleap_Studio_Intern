using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 12.5f, runSpeed = 30f;
    
    public CharacterController myController;
    public Transform myCameraHead;
    public float mouseSensitivity = 100f;
    private float cameraVerticalRotation;
    //adding gravity
    public Vector3 velocity;
    public float gravityModifier;
    //jumping
    public float jumpHeight = 10f;
    private bool readyToJump;
    public Transform ground;
    public LayerMask groundLayer;
    public float groundDistance = .5f;
    //crouching
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 bodyScale;
    public Transform myBody;
    private float initialControllerHeight;
    public float crouchSpeed = 10f;
    private bool isCrouching = false;
    private Animator anim;
    //sliding
    private bool isRunning = false, startSliderTimer;
    private float currentSliderTimer, maxSlideTime = 2f;
    public float slideSpeed = 30f;

    //Hook Shot
    public Transform hitPointTransform;
    private Vector3 hookShotPosition;
    public float hookShotSpeed = 5f;
    private Vector3 flyingCharacterMomentum;
    public Transform grapplingHook;
    private float hookShotSize;

    //Player States
    private State state;
    private enum State { Normal, HookShotFlyingPlayer, HookShotThrown}
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bodyScale = myBody.localScale;
        initialControllerHeight = myController.height;

        state = State.Normal;
        grapplingHook.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Normal:
                PlayerMovement();
                RotateCamera();
                Jump();
                Crouch();
                SlideCounter();
                HandleHookStart();
                break;

            case State.HookShotFlyingPlayer:
                RotateCamera();
                HandleHookShotMovement();
                break;

            case State.HookShotThrown:
                PlayerMovement();
                RotateCamera();
                ThrowHook();
                break;
            default:
                break;
        }

        
        
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouching();
            
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || currentSliderTimer > maxSlideTime)
        {
            StopCrouching();

        }
    }

    private void StopCrouching()
    {
        currentSliderTimer = 0f;
        velocity = Vector3.zero;
        startSliderTimer = false;
        myBody.localScale = bodyScale;
        myCameraHead.position += new Vector3(0, .4f, 0);
        myController.height = initialControllerHeight;
        isCrouching = false;
    }

    private void StartCrouching()
    {
        myBody.localScale = crouchScale;
        myCameraHead.position -= new Vector3(0, .4f, 0);
        myController.height /= 2;
        isCrouching = true;

        if (isRunning)
        {
            velocity = Vector3.ProjectOnPlane(myCameraHead.transform.forward, Vector3.up).normalized * slideSpeed * Time.deltaTime;
            startSliderTimer = true;
        }
    }

    private void Jump()
    {
        readyToJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;
        if (Input.GetButtonDown("Jump") && readyToJump)
        {
            AudioManager.instance.PlayerSFX(2);
            velocity.y = MathF.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        }
        myController.Move(velocity);
    }

    

    private void RotateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.Rotate(Vector3.up * mouseX);

        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            movement = movement * runSpeed * Time.deltaTime;
            isRunning = true;
        }
        else if (isCrouching)
        {
            movement = movement * crouchSpeed * Time.deltaTime;
        }
        else
        {

            movement = movement * speed * Time.deltaTime;
            isRunning = false;
        }

        anim.SetFloat("PlayerSpeed", movement.magnitude);

        movement += flyingCharacterMomentum * Time.deltaTime;

        myController.Move(movement);

        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;

        if (myController.isGrounded)
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
        }

        myController.Move(velocity);

        if (flyingCharacterMomentum.magnitude > 0f)
        {
            float reductionAmount = 4f;
            flyingCharacterMomentum -= flyingCharacterMomentum * reductionAmount * Time.deltaTime;
            if (flyingCharacterMomentum.magnitude < 5f)
            {
                flyingCharacterMomentum = Vector3.zero;
            }
        }
    }

    private void SlideCounter()
    {
        if (startSliderTimer)
        {
            currentSliderTimer += Time.deltaTime;
        }
    }

    private void HandleHookStart()
    {
        if (TestInputDownHookShot())
        {
            RaycastHit hit;

            if (Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit))
            {
                hitPointTransform.position = hit.point;
                hookShotPosition = hit.point;

                hookShotSize = 0f;
                grapplingHook.gameObject.SetActive(true);
                state = State.HookShotThrown;
            }
        }
    }

    private void HandleHookShotMovement()
    {
        grapplingHook.LookAt(hookShotPosition);

        //direction
        Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;

        float hookShotMinSpeed = 12f, hookShotMaxSpeed = 50f;

        float hookShotSpeedModifier = Mathf.Clamp(Vector3.Distance(transform.position, hookShotPosition), hookShotMinSpeed, hookShotMaxSpeed);

        myController.Move(hookShotDirection * hookShotSpeed * hookShotSpeedModifier * Time.deltaTime);

        if (Vector3.Distance(transform.position, hookShotPosition) < 2f)
        {
            StopHookShot();
        }
        if (TestInputDownHookShot())
        {
            StopHookShot();
        }

        if (TestInputJump())
        {
            float extraMomentum = 40f, jumpSpeedUp = 70f;
            flyingCharacterMomentum += hookShotDirection * hookShotSpeed * extraMomentum;
            flyingCharacterMomentum += Vector3.up * jumpSpeedUp;

            StopHookShot();
        }
    }

    private void ThrowHook()
    {
        grapplingHook.LookAt(hookShotPosition);

        float hookShotThrowSpeed = 60f;
        hookShotSize += hookShotThrowSpeed * Time.deltaTime;
        grapplingHook.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookShotPosition))
        {
            state = State.HookShotFlyingPlayer;
        }
    }

    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }


    private bool TestInputDownHookShot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void ResetGravity()
    {
        velocity.y = 0f;
    }

    private void StopHookShot()
    {
        grapplingHook.gameObject.SetActive(false);
        state = State.Normal;
        ResetGravity();
    }
}
