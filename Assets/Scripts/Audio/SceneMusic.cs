using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [Header("Chọn nhạc cho Scene này")]
    [SerializeField] private AudioClip sceneMusic;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(sceneMusic);
        }
    }
}
