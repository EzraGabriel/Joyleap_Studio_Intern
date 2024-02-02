using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    private AudioSource footstepSound;

    [SerializeField]
    private AudioClip[] footstepClips;

    private CharacterController characterController;

    [HideInInspector]
    public float volumeMinimum, volumeMaximum;

    private float accumulateddistance;

    [HideInInspector]
    public float stepDistance;
    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();

        characterController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstepSound();
    }

    private void CheckToPlayFootstepSound()
    {
        if(!characterController.isGrounded)
        {
            return;
        }
        if(characterController.velocity.sqrMagnitude > 0)
        {
            accumulateddistance += Time.deltaTime;

            if(accumulateddistance > stepDistance)
            {
                footstepSound.volume = UnityEngine.Random.Range(volumeMinimum, volumeMaximum);
                footstepSound.clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];
                footstepSound.Play();

                accumulateddistance = 0f;
            }
        }
        else
        {
            accumulateddistance = 0f;
        }
    }
}
