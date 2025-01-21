using UnityEngine;

/// <summary>
/// Controls visual feedback and animations for the player character.
/// Handles squash and stretch animations, particles, and audio effects.
/// </summary>
public class SquareAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform visualTransform;

    [Header("Squash and Stretch Settings")]
    [Tooltip("Vertical stretch factor when jumping")]
    [SerializeField] private float jumpStretchValue = 1.3f;

    [Tooltip("Vertical squash factor when landing")]
    [SerializeField] private float landSquashValue = 0.7f;

    [Tooltip("Speed of squash/stretch animations")]
    [SerializeField] private float animationSpeed = 8f;

    [Header("Tilt Settings")]
    [Tooltip("Maximum tilt angle during movement")]
    [SerializeField] private float maxTiltAngle = 15f;

    [Tooltip("How fast the square tilts")]
    [SerializeField] private float tiltSpeed = 5f;

    private PlayerController playerController; // Reference to PlayerController

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float verticalVelocity;
    private bool hasPlayedJumpSound = false;

    /// <summary>
    /// Initializes required components and stores original scale
    /// </summary>
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;

        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>(); // Get reference to PlayerController

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalScale = visualTransform.localScale;
        targetScale = originalScale;
    }

    /// <summary>
    /// Handles animation updates and checks for jump conditions
    /// </summary>
    private void Update()
    {
        // Check if reached peak of jump
        if (rb.velocity.y < 0 && verticalVelocity >= 0)
        {
            targetScale = originalScale;
            hasPlayedJumpSound = false; // Reset flag when falling
        }

        // Store current vertical velocity for next frame comparison
        verticalVelocity = rb.velocity.y;

        // Apply smooth scale interpolation
        visualTransform.localScale = Vector3.Lerp(
            visualTransform.localScale,
            targetScale,
            Time.deltaTime * animationSpeed
        );

        // Check for jump animation conditions
        if (rb.velocity.y > 1f && !isGrounded && !hasPlayedJumpSound)
        {
            ApplyJumpStretch();
            PlayJumpEffects();
            hasPlayedJumpSound = true; // Set flag after playing jump sound
        }
        // Apply tilt based on horizontal movement
        ApplyMovementTilt();
    }

    /// <summary>
    /// Handles collision detection for landing effects
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y > 0)
        {
            isGrounded = true;
            hasPlayedJumpSound = false; // Reset flag on landing
            ApplyLandSquash();
            PlayLandingEffects();

            Invoke(nameof(ResetScale), 0.1f);
        }
    }

    /// <summary>
    /// Tracks when object leaves the ground
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    /// <summary>
    /// Applies tilt effect based on horizontal movement
    /// </summary>
    private void ApplyMovementTilt()
    {
        float horizontalVelocity = rb.velocity.x;
        float targetRotation = 0f;

        if (Mathf.Abs(horizontalVelocity) > 0.1f)
        {
            // Added positive sign to reverse the tilt direction
            targetRotation = horizontalVelocity * maxTiltAngle / playerController.moveSpeed;
        }

        // Smoothly rotate the visual transform
        Quaternion currentRotation = visualTransform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(0, 0, targetRotation);
        visualTransform.rotation = Quaternion.Lerp(currentRotation, targetQuaternion, Time.deltaTime * tiltSpeed);
    }

    /// <summary>
    /// Applies stretch effect for jumping
    /// </summary>
    private void ApplyJumpStretch()
    {
        targetScale = new Vector3(
            originalScale.x * (1f / jumpStretchValue),
            originalScale.y * jumpStretchValue,
            originalScale.z
        );
    }

    /// <summary>
    /// Applies squash effect for landing
    /// </summary>
    private void ApplyLandSquash()
    {
        targetScale = new Vector3(
            originalScale.x * (1f / landSquashValue),
            originalScale.y * landSquashValue,
            originalScale.z
        );
    }

    /// <summary>
    /// Plays jump-related particle and audio effects
    /// </summary>
    private void PlayJumpEffects()
    {
        if (jumpParticles != null && !jumpParticles.isPlaying)
            jumpParticles.Play();

        if (jumpSound != null && audioSource != null)
        {
            audioSource.volume = 1.2f; // Slightly louder jump sound
            audioSource.PlayOneShot(jumpSound);
        }
    }

    /// <summary>
    /// Plays landing-related particle and audio effects
    /// </summary>
    private void PlayLandingEffects()
    {
        if (landParticles != null)
        {
            float impactForce = Mathf.Abs(rb.velocity.y);
            landParticles.transform.localScale = Vector3.one * Mathf.Clamp(impactForce / 10f, 0.5f, 2f);
            landParticles.Play();
        }

        if (landSound != null && audioSource != null)
        {
            audioSource.volume = 1f; // Normal volume for landing
            audioSource.PlayOneShot(landSound);
        }
    }

    /// <summary>
    /// Resets visual scale to original values
    /// </summary>
    private void ResetScale()
    {
        targetScale = originalScale;
    }
}