using UnityEngine;

/// <summary>
/// Controls platform behavior and physics interactions.
/// Handles bounce mechanics for the player character.
/// </summary>
public class Platform : MonoBehaviour
{
    [Header("Physics Settings")]
    [Tooltip("Force applied when player bounces off platform")]
    [SerializeField] private float jumpForce = 10f;

    [Tooltip("Maximum allowed collision velocity")]
    [SerializeField] private float maxCollisionVelocity = 20f;

    /// <summary>
    /// Handles collision detection and bounce mechanics.
    /// </summary>
    /// <param name="collision">Collision information containing colliding object data</param>
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
    /// Applies vertical bounce force with safety checks.
    /// </summary>
    private void ApplyBounceForce(Rigidbody2D rb)
    {
        Vector2 velocity = rb.velocity;

        // Preserve current horizontal velocity
        float currentHorizontalVelocity = velocity.x;

        // Apply bounce force with clamping
        velocity.y = Mathf.Clamp(jumpForce, -maxCollisionVelocity, maxCollisionVelocity);

        // Restore horizontal velocity
        velocity.x = currentHorizontalVelocity;

        rb.velocity = velocity;
    }
}