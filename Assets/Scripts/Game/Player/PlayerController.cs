﻿using UnityEngine;

/// <summary>
/// Controls the player's movement and physics interactions.
/// Supports both mobile (Android) and keyboard input for testing.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed of the player")]
    [SerializeField] public float moveSpeed = 5f;

    [Tooltip("Reduced speed during penalty phase")]
    [SerializeField] private float penaltyMoveSpeed = 2f;

    [Tooltip("Duration of penalty in seconds")]
    [SerializeField] private float penaltyDuration = 3f;

    [Header("Size Reduction Settings")]
    [Tooltip("The scale factor for the player when penalized")]
    [SerializeField] private Vector3 penaltyScale = new Vector3(0.5f, 0.5f, 1f);

    [Tooltip("Original player scale (default size)")]
    private Vector3 originalScale;

    [Header("Death Settings")]
    [Tooltip("Reference to the Death script for handling death logic")]
    private Death deathScript;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    private float moveX;
    private Vector2 touchStartPosition;
    private bool isTouching;

    // Penalty state variables
    private bool isPenalized = false;
    private bool isSmall = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        deathScript = GetComponent<Death>();

        if (deathScript == null)
        {
            Debug.LogError("Death script not found on the Player GameObject.");
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleKeyboardInput();
#elif UNITY_ANDROID
        HandleMobileInput();
#endif
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = moveX;
        rb.velocity = velocity;
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetMouseButton(0)) // Detect mouse input
        {
            if (!isTouching)
            {
                touchStartPosition = Input.mousePosition;
                isTouching = true;
            }
            else
            {
                float swipeDelta = Input.mousePosition.x - touchStartPosition.x;
                moveX = Mathf.Clamp(swipeDelta / Screen.width * moveSpeed, -moveSpeed, moveSpeed);
            }
        }
        else if (isTouching)
        {
            isTouching = false;
            moveX = 0f;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0) moveX = horizontalInput * moveSpeed;
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    isTouching = true;
                    break;

                case TouchPhase.Moved:
                    if (isTouching)
                    {
                        float swipeDelta = touch.position.x - touchStartPosition.x;
                        moveX = Mathf.Clamp(swipeDelta / Screen.width * moveSpeed, -moveSpeed, moveSpeed);
                    }
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    moveX = 0f;
                    break;
            }
        }
    }

    public void ApplyPenalty()
    {
        if (!isPenalized)
        {
            StartCoroutine(PenaltyCoroutine());
        }
        else
        {
            deathScript?.TriggerDeath(); // Trigger death directly from the Death script
        }
    }

    private System.Collections.IEnumerator PenaltyCoroutine()
    {
        isPenalized = true;

        if (!isSmall)
        {
            // First penalty: reduce size and movement speed
            isSmall = true;
            transform.localScale = penaltyScale;
            float originalSpeed = moveSpeed;
            moveSpeed = penaltyMoveSpeed;

            yield return new WaitForSeconds(penaltyDuration);

            // Restore original size and speed
            transform.localScale = originalScale;
            moveSpeed = originalSpeed;

            isPenalized = false;
        }
    }

    /// <summary>
    /// Resets the player to its original state. Useful for debugging or respawning.
    /// </summary>
    public void ResetPlayer()
    {
        isSmall = false;
        isPenalized = false;
        transform.localScale = originalScale;
        moveSpeed = 5f;
    }
}
