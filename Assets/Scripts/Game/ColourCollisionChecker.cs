using UnityEngine;

public class ColorCollisionChecker : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        // Ensure the player has a SpriteRenderer component
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer missing on the player object!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is tagged as "Platform"
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Get the platform's SpriteRenderer
            SpriteRenderer platformSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (platformSpriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer missing on the platform object!");
                return;
            }

            // Compare the sprites of the player and the platform
            if (playerSpriteRenderer.sprite == platformSpriteRenderer.sprite)
            {
                // Colors match; player can pass through or bounce
                Debug.Log("Colors match! Interaction allowed.");
            }
            else
            {
                // Colors don’t match; block the player
                Debug.Log("Colors don't match! Blocking the player.");
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero; // Stop the player's movement
                }
            }
        }
    }
}
