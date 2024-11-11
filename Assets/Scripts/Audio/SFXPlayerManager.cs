using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerManager : Singleton<SFXPlayerManager>
{
    [SerializeField] AudioSource SFXSource;
    public AudioClip Player_Dash;
    public AudioClip Player_Attack1;
    public AudioClip Player_Attack2;
    public AudioClip Player_Attack3;
    public AudioClip Player_Death;
    public AudioClip Player_Skill;
    public AudioClip Player_GetHit;
    void Start(){
        SFXSource = GetComponent<AudioSource>();
    }
    internal void PlaySFX(AudioClip clip)
    {
        if (SFXSource && clip) 
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
