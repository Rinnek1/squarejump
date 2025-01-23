using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public float fallThreshold = -10.0f; // Y-position threshold for falling off-screen

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag==("Death"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
