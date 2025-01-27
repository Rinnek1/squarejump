using UnityEngine;

/// <summary>
/// Adds horizontal movement to platforms.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    private float movementRange;
    private float speed;

    private Vector3 startPosition;
    private bool movingRight = true;

    public void SetMovementRange(float range, float platformSpeed)
    {
        movementRange = range;
        speed = platformSpeed;
        startPosition = transform.position;
    }

    private void Update()
    {
        float movementDelta = speed * Time.deltaTime;
        if (movingRight)
        {
            transform.position += new Vector3(movementDelta, 0, 0);
            if (transform.position.x > startPosition.x + movementRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position -= new Vector3(movementDelta, 0, 0);
            if (transform.position.x < startPosition.x - movementRange)
            {
                movingRight = true;
            }
        }
    }
}
