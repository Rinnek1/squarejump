using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    [Header("Sprites")]
    [Tooltip("Primary sprite (e.g., red square)")]
    public Sprite sprite1;

    [Tooltip("Secondary sprite (e.g., blue square)")]
    public Sprite sprite2;

    private SpriteRenderer spriteRenderer;

    // Double-tap detection
    private float lastTapTime = 0f;
    [Tooltip("Maximum time interval (in seconds) between taps to register a double-tap")]
    public float doubleTapThreshold = 0.3f; // Adjust this value for sensitivity

    /// <summary>
    /// Initialize the SpriteRenderer by finding it in the child object.
    /// </summary>
    private void Awake()
    {
        // Get the SpriteRenderer from the child object (Visual)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is missing in the child! Please ensure a SpriteRenderer is attached to the Visual child object.");
            return;
        }

        // Set the initial sprite
        spriteRenderer.sprite = sprite1;
    }

    /// <summary>
    /// Detects input and toggles sprite on double-tap.
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect screen tap or mouse click
        {
            HandleDoubleTap();
        }
    }

    /// <summary>
    /// Detects a double-tap and toggles the sprite if detected.
    /// </summary>
    private void HandleDoubleTap()
    {
        float currentTime = Time.time; // Get the current time

        // Check if this tap is within the double-tap threshold
        if (currentTime - lastTapTime <= doubleTapThreshold)
        {
            // Double-tap detected, toggle the sprite
            ToggleSprite();
        }

        // Update the last tap time
        lastTapTime = currentTime;
    }

    /// <summary>
    /// Toggles the sprite between sprite1 and sprite2.
    /// </summary>
    private void ToggleSprite()
    {
        // Get the current sprite
        Sprite currentSprite = spriteRenderer.sprite;

        // Switch to the other sprite
        if (currentSprite == sprite1)
        {
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    /// <summary>
    /// Returns the current sprite of the player.
    /// </summary>
    public Sprite GetCurrentSprite()
    {
        return spriteRenderer.sprite;
    }
}
