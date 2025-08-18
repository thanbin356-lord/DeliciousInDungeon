using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("------Audio Clips------")]
    public AudioClip enemyBackgroundMusic;
    public AudioClip background;
    public AudioClip Dragon_Fire;
    public AudioClip Dragon_Stomp;
    public AudioClip Slime_BM;
    public AudioClip Demon_BM;

    public static AudioManager Instance;

    private AudioClip currentMusicClip; // nhớ bài nhạc hiện tại

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
        PlayMusic(background);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (clip == currentMusicClip) return;

        currentMusicClip = clip;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource)
        {
            musicSource.Stop();
            currentMusicClip = null;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource && clip)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void PlayNormalBackgroundMusic()
    {
        PlayMusic(background);
    }

    public void PlayEnemyBackgroundMusic()
    {
        PlayMusic(enemyBackgroundMusic);
    }
}
