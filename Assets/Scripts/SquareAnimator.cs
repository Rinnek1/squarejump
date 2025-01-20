using UnityEngine;

/// <summary>
/// Controls visual feedback and animations for the player character.
/// Handles particles, sprite flipping, and audio effects.
/// </summary>
public class SquareAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;
    [SerializeField] private ParticleSystem moveParticles;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;

    /// <summary>
    /// Initializes required components
    /// </summary>
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Handles collision detection for ground check and landing effects
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            isGrounded = true;
            PlayLandingEffects();
        }
    }

    /// <summary>
    /// Handles collision exit for jump effects
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        PlayJumpEffects();
    }

    /// <summary>
    /// Plays visual and audio effects for jumping
    /// </summary>
    private void PlayJumpEffects()
    {
        if (jumpParticles != null)
            jumpParticles.Play();

        if (jumpSound != null && audioSource != null)
            audioSource.PlayOneShot(jumpSound);
    }

    /// <summary>
    /// Plays visual and audio effects for landing
    /// </summary>
    private void PlayLandingEffects()
    {
        if (landParticles != null)
        {
            // Scale particles based on impact velocity
            float impactForce = Mathf.Abs(rb.velocity.y);
            landParticles.transform.localScale = Vector3.one * Mathf.Clamp(impactForce / 10f, 0.5f, 2f);
            landParticles.Play();
        }

        if (landSound != null && audioSource != null)
            audioSource.PlayOneShot(landSound);
    }

    /// <summary>
    /// Updates movement particles based on velocity
    /// </summary>
    private void Update()
    {
        if (moveParticles != null)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded)
            {
                if (!moveParticles.isPlaying) moveParticles.Play();

                // Flip particle direction based on movement
                float direction = Mathf.Sign(rb.velocity.x);
                moveParticles.transform.localScale = new Vector3(direction, 1, 1);
            }
            else
            {
                if (moveParticles.isPlaying) moveParticles.Stop();
            }
        }
    }
}