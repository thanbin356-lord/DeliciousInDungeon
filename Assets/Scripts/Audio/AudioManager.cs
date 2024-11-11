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
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        SFXSource = GetComponent<AudioSource>();
        PlayNormalBackgroundMusic();
    }

    public void PlayNormalBackgroundMusic()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayEnemyBackgroundMusic()
    {
        musicSource.clip = enemyBackgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    internal void PlaySFX(AudioClip clip)
    {
        if (SFXSource && clip)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
