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

    [Header("Effects")]
    [SerializeField] private ParticleSystem bounceEffect;
    [SerializeField] private AudioClip bounceSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Handles collision detection and bounce mechanics.
    /// </summary>
    /// <param name="collision">Collision information containing colliding object data</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdź czy obiekt porusza się w dół
        if (collision.relativeVelocity.y <= 0f)
        {
            ProcessCollision(collision);
            PlayBounceEffects();
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

        // Zachowaj obecną prędkość poziomą
        float currentHorizontalVelocity = velocity.x;

        // Zastosuj siłę odbicia z ograniczeniem
        velocity.y = Mathf.Clamp(jumpForce, -maxCollisionVelocity, maxCollisionVelocity);

        // Przywróć prędkość poziomą
        velocity.x = currentHorizontalVelocity;

        rb.velocity = velocity;
    }

    private void PlayBounceEffects()
    {
        if (bounceEffect != null) bounceEffect.Play();
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
    }
}