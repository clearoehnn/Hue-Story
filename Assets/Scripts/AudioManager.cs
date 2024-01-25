using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music Source")]
    public AudioSource musicSource;
    
    [Header("SFX Source")]
    public AudioSource sfxSource;
    
    [Header("Clips")]
    public AudioClip doorOpen;
    public AudioClip collectCollectible;
    public AudioClip sceneTransition;
    
    [Header("Other")]
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(string audioClipName)
    {
        switch (audioClipName)
        {
            case "doorOpen":
                sfxSource.clip = doorOpen;
                break;
            case "collectCollectible":
                sfxSource.clip = collectCollectible;
                break;
            case "sceneTransition":
                sfxSource.clip = sceneTransition;
                break;
        }
        sfxSource.Play();
    }
}
