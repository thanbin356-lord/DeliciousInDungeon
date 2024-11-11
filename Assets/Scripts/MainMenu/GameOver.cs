using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen; // Panel Game Over (kéo thả vào Inspector)
    [SerializeField] private float delayBeforeDisplay = 1f; // Trễ trước khi hiện panel
    [SerializeField] private GameObject objectToDestroy;

    public void ShowGameOverScreen()
    {
        StartCoroutine(DisplayGameOver());
    }

    private IEnumerator DisplayGameOver()
    {
        if (delayBeforeDisplay > 0)
            yield return new WaitForSeconds(delayBeforeDisplay);

        gameOverScreen.SetActive(true);
    }
    public void MainMenu()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        SceneManager.LoadScene("Main Menu");
    }
}
