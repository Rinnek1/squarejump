using UnityEngine;

/// <summary>
/// Camera follows a target object smoothly.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Object that camera will follow")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [Tooltip("How smoothly the camera follows the target")]
    [SerializeField] private float smoothSpeed = 0.125f;

    [Tooltip("Offset from target position")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    /// <summary>
    /// Follows target position with smooth movement.
    /// Only follows when target is above camera position.
    /// </summary>
    private void LateUpdate()
    {
        if (target != null && target.position.y > transform.position.y)
        {
            // Keep current X and Z positions, only update Y
            Vector3 newPosition = new Vector3(
                transform.position.x,
                target.position.y,
                transform.position.z
            );

            // Apply smooth movement
            transform.position = Vector3.Lerp(
                transform.position,
                newPosition + offset,
                smoothSpeed
            );
        }
    }
}