using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgm;

    public AudioSource[] SFXs;
    private void Awake()
    {
        instance = this;
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlayerSFX(int sfxNumber)
    {
        SFXs[sfxNumber].Stop();
        SFXs[sfxNumber].Play();
    }
}
