using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < 0 || viewportPosition.y > 1 || viewportPosition.x < 0 || viewportPosition.x > 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
