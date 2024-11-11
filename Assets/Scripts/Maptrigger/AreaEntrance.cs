using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
   [SerializeField] private string transitionName;

    private void Start()
    {
        if (SceneManagement.Instance != null && PlayerController.Instance != null && transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = transform.position;

            CameraController.Instance?.SetPlayerCameraFollw();
        }
        else
        {
            Debug.LogWarning("AreaEntrance: SceneManagement, PlayerController, or CameraController not found or conditions not met."); 
        }
    }
}
