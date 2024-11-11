using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatSceneManager : Singleton<DefeatSceneManager>
{
    [SerializeField] private string defeatSceneName = "GameOverScene";
    [SerializeField] private float delayBeforeLoading = 1f;

    protected override void Awake()
    {
        base.Awake();
    }
    public void LoadDefeatScene()
    {
        StartCoroutine(LoadDefeatSceneCoroutine());
    }
    private IEnumerator LoadDefeatSceneCoroutine()
    {
        if (delayBeforeLoading > 0)
        {
            yield return new WaitForSeconds(delayBeforeLoading);
        }

        // Reset Player Before Loading
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.ResetSprite();
            PlayerController.Instance.transform.position = Vector3.zero;
        }

        // Destroy all objects in the scene
        StopAllCoroutines(); // Stop coroutines on this object
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj != gameObject && obj.GetComponent<LeanTween>() == null)
            {
                Destroy(obj);
            }
        }
        Destroy(gameObject);

        SceneManager.LoadScene(defeatSceneName);
    }
}
