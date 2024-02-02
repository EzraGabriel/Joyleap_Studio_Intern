using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeWooshSound : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] whooshSounds;

    void PlayWhooshSounds()
    {
        audioSource.clip = whooshSounds[Random.Range(0, whooshSounds.Length)];
        audioSource.Play();
    }
}
