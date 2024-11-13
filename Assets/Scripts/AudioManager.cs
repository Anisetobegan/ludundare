using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [SerializeField] AudioClip gameplayClip;
    [SerializeField] AudioClip mainMenuClip;

    public static AudioManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        if (musicAudioSource != null)
        {
            if (mainMenuClip == null)
            {
                musicAudioSource.clip = gameplayClip;
            }
            else
            {
                musicAudioSource.clip = mainMenuClip;
            }
            musicAudioSource.Play();
        }        
    }

    public void PlaySFX (AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
