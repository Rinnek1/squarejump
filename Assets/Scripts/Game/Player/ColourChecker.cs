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
        // Check if the object colliding is the player
        ColorSwitcher playerColorSwitcher = collision.gameObject.GetComponent<ColorSwitcher>();

        if (playerColorSwitcher != null)
        {
            Color playerColor = playerColorSwitcher.GetCurrentColor();

            // Ensure the player is falling onto the platform
            if (collision.relativeVelocity.y <= 0f)
            {
                if (playerColor != platformColor)
                {
                    // Delay destruction slightly to allow bounce if needed
                    Invoke(nameof(DestroyPlatform), 0.2f);
                }
            }
        }
    }

    /// <summary>
    /// Destroys the platform.
    /// </summary>
    private void DestroyPlatform()
    {
        Destroy(gameObject);
    }
}
