using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public float fallThreshold = -10.0f; // Y-position threshold for falling off-screen
    //public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
