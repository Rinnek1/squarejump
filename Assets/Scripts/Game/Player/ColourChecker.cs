using UnityEngine;

/// <summary>
/// Checks color alignment between the player and the platform.
/// </summary>
public class ColorChecker : MonoBehaviour
{
    private Color platformColor;

    private void Awake()
    {
        SpriteRenderer platformSprite = GetComponent<SpriteRenderer>();
        if (platformSprite != null)
        {
            platformColor = platformSprite.color;
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on the platform object!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColorSwitcher playerColorSwitcher = collision.gameObject.GetComponent<ColorSwitcher>();
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerColorSwitcher != null && playerController != null)
        {
            // Ensure the player is falling onto the platform
            if (collision.relativeVelocity.y <= 0f)
            {
                Color playerColor = playerColorSwitcher.GetCurrentColor();

                if (playerColor != platformColor)
                {
                    // Apply penalty: stun the player and reduce movement temporarily
                    playerController.ApplyPenalty();

                    // Optionally, destroy the platform after penalty
                    Invoke(nameof(DestroyPlatform), 0.5f);
                }
            }
        }
    }

    private void DestroyPlatform()
    {
        Destroy(gameObject);
    }
}
