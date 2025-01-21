using UnityEngine;

/// <summary>
/// Controls the player's movement and physics interactions.
/// Supports both mobile (Android) and keyboard input for testing.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed of the player")]
    public float moveSpeed = 5f;

    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;

    // Input variables
    private float moveX;
    private Vector2 touchStartPosition;
    private bool isTouching;

    /// <summary>
    /// Initializes required components
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles input detection
    /// </summary>
    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
    // Keyboard input for testing in Unity editor
    if (Input.GetMouseButton(0)) // 
    {
        if (!isTouching)
        {
            //  TouchPhase.Began
            touchStartPosition = Input.mousePosition;
            isTouching = true;
        }
        else
        {
            //TouchPhase.Moved
            float swipeDelta = Input.mousePosition.x - touchStartPosition.x;
            moveX = Mathf.Clamp(swipeDelta / Screen.width * moveSpeed, -moveSpeed, moveSpeed);
        }
    }
    else if (isTouching)
    {
        //  TouchPhase.Ended
        isTouching = false;
        moveX = 0f;
    }

    // Zachowujemy też sterowanie klawiaturą
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    if (horizontalInput != 0) moveX = horizontalInput * moveSpeed;
#elif UNITY_ANDROID
    HandleMobileInput();
#endif
    }

    /// <summary>
    /// Applies physics-based movement
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = moveX;
        rb.velocity = velocity;
    }

    /// <summary>
    /// Processes mobile touch input for movement
    /// </summary>
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
}