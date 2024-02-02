using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private Transform lookRoot;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;

    private bool isCrouching;

    private PlayerFootstep playerFootsteps;

    private float sprintVolume = 1f;
    private float crouchVolume = .1f;
    private float walkVolumeMin = .2f, walkVolumeMax = .6f;

    private float walkStepDistance = .4f;
    private float sprintStepDistance = .25f;
    private float crouchStepDistance = .5f;

    private PlayerStats playerStats;
    private float sprintValue = 100f;
    public float sprintTreshold = 10f;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        lookRoot = transform.GetChild(0);

        playerFootsteps = GetComponentInChildren<PlayerFootstep>();

        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        playerFootsteps.volumeMinimum = walkVolumeMin;
        playerFootsteps.volumeMaximum = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepDistance;
    }
    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if(sprintValue > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
            {
                playerMovement.speed = sprintSpeed;
                playerFootsteps.stepDistance = sprintStepDistance;

                playerFootsteps.volumeMinimum = sprintVolume;
                playerFootsteps.volumeMaximum = sprintVolume;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;

            playerFootsteps.stepDistance = walkStepDistance;
            playerFootsteps.volumeMinimum = walkVolumeMin;
            playerFootsteps.volumeMaximum = walkVolumeMax;
            
        }
        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            sprintValue -= sprintTreshold * Time.deltaTime;

            if(sprintValue <= 0f)
            {
                sprintValue = 0f;
                playerMovement.speed = moveSpeed;
                playerFootsteps.stepDistance = walkStepDistance;
                playerFootsteps.volumeMinimum = walkVolumeMin;
                playerFootsteps.volumeMaximum = walkVolumeMax;
            }
            playerStats.DisplayStaminaStats(sprintValue);
        }
        else
        {
            if(sprintValue != 100f)
            {
                sprintValue += (sprintTreshold / 2f) * Time.deltaTime;
                playerStats.DisplayStaminaStats(sprintValue);
                if(sprintValue > 100f)
                {
                    sprintValue = 100f;
                }
            }
        }
    }

    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(isCrouching)
            {
                lookRoot.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovement.speed = moveSpeed;
                

                isCrouching = false;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovement.speed = moveSpeed;

                playerFootsteps.stepDistance = crouchStepDistance;
                playerFootsteps.volumeMinimum = crouchVolume;
                playerFootsteps.volumeMaximum = crouchVolume;

                isCrouching = true;
            }
        }
    }
}
