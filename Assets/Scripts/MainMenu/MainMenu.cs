using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject rawImage;
    public VideoPlayer videoPlayer;
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            rawImage.SetActive(false);
            ResetVideo();
        }
    }
   public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Guide()
    {
        rawImage.SetActive(true);    
        videoPlayer.Play();
    }
    public void ResetVideo(){
        videoPlayer.Stop();
        videoPlayer.time= 0;
    }
}
