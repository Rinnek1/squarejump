using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color primaryColor = new Color(0.976f, 0.380f, 0.404f); // #F96167
    [SerializeField] private Color secondaryColor = new Color(0.976f, 0.906f, 0.584f); // #F9E795

    private SpriteRenderer spriteRenderer;
    private float lastTapTime = 0f;
    [SerializeField] private float doubleTapThreshold = 0.3f;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer missing in Visual child object!");
            return;
        }
        spriteRenderer.color = primaryColor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleDoubleTap();
        }
    }

    private void HandleDoubleTap()
    {
        float currentTime = Time.time;
        if (currentTime - lastTapTime <= doubleTapThreshold)
        {
            ToggleColor();
        }
        lastTapTime = currentTime;
    }

    private void ToggleColor()
    {
        spriteRenderer.color = spriteRenderer.color == primaryColor ? secondaryColor : primaryColor;
    }

    public Color GetCurrentColor()
    {
        return spriteRenderer.color;
    }
}