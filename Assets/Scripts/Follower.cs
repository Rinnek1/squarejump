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

    private readonly float cameraZPosition = -10f;

    /// <summary>
    /// Follows target position with smooth movement.
    /// Only follows when target is above camera position.
    /// </summary>
    private void LateUpdate()
    {
        if (target != null && target.position.y > transform.position.y)
        {
            Debug.Log($"Camera position: {transform.position}");
            Debug.Log($"Target position: {target.position}");
            // Keep current X and Z positions, only update Y
            Vector3 newPosition = new Vector3(
                transform.position.x,
                target.position.y,
                cameraZPosition
            );

            // Apply smooth movement
            transform.position = Vector3.Lerp(
                transform.position,
                newPosition,
                smoothSpeed
            );
        }
    }
}