using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneTransitionOnTimelineEnd : MonoBehaviour
{
    public PlayableDirector timeline;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

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
            SceneManager.LoadScene(sceneToLoad);
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        }
    }
}