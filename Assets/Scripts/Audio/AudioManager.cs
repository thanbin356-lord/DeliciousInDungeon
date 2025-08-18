using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Sorce------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------Audio ------")]
    public AudioClip enemyBackgroundMusic;
    public AudioClip background;
    public AudioClip Dragon_Fire;
    public AudioClip Dragon_Stomp;
    public AudioClip Slime_BM;
    public AudioClip Demon_BM;
    public static AudioManager Instance;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayNormalBackgroundMusic();
    }

    public void PlayNormalBackgroundMusic()
    {
        if (musicSource)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayEnemyBackgroundMusic()
    {
        if (musicSource)
        {
            musicSource.clip = enemyBackgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource) musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource && clip)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
