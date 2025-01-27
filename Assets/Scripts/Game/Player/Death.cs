using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load upon death")]
    [SerializeField] private string sceneToLoad = "MainMenu";

    [Header("Boundary Settings")]
    [Tooltip("Offset to allow slight leeway before detecting out-of-bounds")]
    [SerializeField] private float boundaryOffset = 0.1f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene.");
        }
    }

    private void Update()
    {
        CheckOutOfBounds();
    }

    /// <summary>
    /// Checks if the player has moved out of the camera's visible viewport and triggers death logic.
    /// </summary>
    private void CheckOutOfBounds()
    {
        if (mainCamera == null) return;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < -boundaryOffset || viewportPosition.y > 1 + boundaryOffset ||
            viewportPosition.x < -boundaryOffset || viewportPosition.x > 1 + boundaryOffset)
        {
            TriggerDeath();
        }
    }

    /// <summary>
    /// Handles collisions with death-triggering objects.
    /// </summary>
    /// <param name="collision">Collider of the object the player collided with.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            TriggerDeath();
        }
    }

    /// <summary>
    /// Executes the death logic, such as reloading the scene.
    /// </summary>
    public void TriggerDeath() // Changed to public
    {
        Debug.Log("Player died!");
        SceneManager.LoadScene(sceneToLoad);
    }
}
