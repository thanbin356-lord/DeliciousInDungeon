using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMS : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnEnable()
    {
        if (audioManager != null)
        {
            audioManager.PlayEnemyBackgroundMusic();
        }
    }

    void OnDisable()
    {
        if (audioManager != null)
        {
            audioManager.StopMusic();
            audioManager.PlayNormalBackgroundMusic();
        }
    }
}