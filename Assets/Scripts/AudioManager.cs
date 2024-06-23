using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip falling;
    public AudioClip nooooVoice;
    public AudioClip laughtVoice;
    public AudioClip yiihaVoice;
    public AudioClip[] footstepSounds;

    [Header("Footstep Settings")]
    [SerializeField] private float minFootstepInterval = 0.3f;
    [SerializeField] private float maxFootstepInterval = 0.6f;

    private float timeSinceLastFootstep;

    public static AudioManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayMusic(backgroundMusic);
    }

    void Update()
    {
        PlayFootstepSFX();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFootstepSFX()
    {
        if (Time.time - timeSinceLastFootstep >= Random.Range(minFootstepInterval, maxFootstepInterval))
        {
            AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            sfxSource.PlayOneShot(footstepSound);
            timeSinceLastFootstep = Time.time;
        }
    }
}
