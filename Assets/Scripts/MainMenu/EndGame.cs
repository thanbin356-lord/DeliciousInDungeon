using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public PlayableDirector timeline; // Reference to your Timeline asset    
    void OnEnable()
    {
        timeline.stopped += OnTimelineStopped;
    }

    void OnDisable()
    {
        timeline.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector aDirector)
    {
        if (aDirector == timeline)
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.hideFlags == HideFlags.None && obj.scene.IsValid())
                {
                    Destroy(obj);
                }
            }
            SceneManager.LoadScene("Main Menu");
        }
    }
}
