using UnityEngine;

/// <summary>
/// Controls platform behavior and physics interactions.
/// Handles bounce mechanics for the player character.
/// </summary>
public class Platform : MonoBehaviour
{
    [Header("Physics Settings")]
    [Tooltip("Base force applied when player bounces off platform")]
    [SerializeField] private float baseJumpForce = 10f;

    [Tooltip("Maximum allowed collision velocity")]
    [SerializeField] private float maxCollisionVelocity = 20f;

    [Tooltip("Bounce force increment every 50 points")]
    [SerializeField] private float bounceForceIncrement = 2f;

    [Tooltip("Maximum cap for bounce force")]
    [SerializeField] private float maxBounceForce = 25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if object is falling onto the platform
        if (collision.relativeVelocity.y <= 0f)
        {
            ProcessCollision(collision);
        }
    }

    /// <summary>
    /// Processes the collision and applies bounce force to player.
    /// </summary>
    private void ProcessCollision(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            ApplyBounceForce(rb);
        }
    }

    /// <summary>
    /// Applies vertical bounce force dynamically based on score.
    /// </summary>
    private void ApplyBounceForce(Rigidbody2D rb)
    {
        Vector2 velocity = rb.velocity;

        // Preserve current horizontal velocity
        float currentHorizontalVelocity = velocity.x;

        // Adjust bounce force based on score
        float scoreMultiplier = ScoreManager.Instance != null ? (ScoreManager.Instance.GetScore() / 50) : 0;
        float adjustedBounceForce = Mathf.Min(baseJumpForce + scoreMultiplier * bounceForceIncrement, maxBounceForce);

        // Apply bounce force with clamping
        velocity.y = Mathf.Clamp(adjustedBounceForce, -maxCollisionVelocity, maxCollisionVelocity);

        // Restore horizontal velocity
        velocity.x = currentHorizontalVelocity;

        rb.velocity = velocity;
    }
}
