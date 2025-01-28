using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls platform generation and management in the game.
/// </summary>
public class PlatformGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject spikeballPrefab;

    [Header("Generation Settings")]
    [Tooltip("Minimum vertical distance between platforms")]
    [SerializeField] private float minY = 1.0f;

    [Tooltip("Maximum vertical distance between platforms")]
    [SerializeField] private float maxY = 2.5f;

    [Tooltip("Horizontal range for platform spawning")]
    [SerializeField] private float levelWidth = 5.0f;

    [Tooltip("Minimum horizontal distance between platforms")]
    [SerializeField] private float minHorizontalDistance = 1.5f;

    [Tooltip("Chance to spawn a spikeball (0 to 1)")]
    [SerializeField] private float spikeballChance = 0.3f;

    [Header("Dynamic Difficulty Settings")]
    [Tooltip("Score threshold to start generating spikeballs")]
    [SerializeField] private int spikeballThreshold = 50;

    [Tooltip("Base chance of spawning platforms with moving behavior (0 to 1)")]
    [SerializeField] private float movingPlatformChance = 0.2f;

    [Tooltip("Increase in moving platform chance for every 50 points")]
    [SerializeField] private float movingPlatformChanceIncrement = 0.05f;

    private readonly Color color1 = new Color(0.976f, 0.380f, 0.404f); // #F96167
    private readonly Color color2 = new Color(0.976f, 0.906f, 0.584f); // #F9E795

    private float highestY;
    private List<Vector3> platformPositions = new List<Vector3>();

    private void Start()
    {
        // Generate initial platforms
        for (int i = 0; i < 10; i++)
        {
            GeneratePlatform(i * maxY);
        }
    }

    private void Update()
    {
        float playerY = Camera.main.transform.position.y;

        // Only generate platforms if the player is approaching the highest point
        while (playerY + 10.0f > highestY)
        {
            float newY = highestY + Random.Range(minY, maxY);
            GeneratePlatform(newY);

            // Properly increment `highestY` to reflect the new platform's Y position
            highestY = newY;
        }
    }

    private void GeneratePlatform(float y)
    {
        // Ensure platforms are not too close horizontally
        Vector3 position;
        bool validPosition;
        int maxAttempts = 10; // Avoid infinite loops in edge cases
        int attempts = 0;

        do
        {
            position = new Vector3(
                Random.Range(-levelWidth / 2, levelWidth / 2),
                y,
                0
            );
            validPosition = IsPositionValid(position);
            attempts++;
        } while (!validPosition && attempts < maxAttempts);

        // Instantiate platform at the valid position
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        platformPositions.Add(position); // Keep track of platform positions

        // Assign random color
        SpriteRenderer platformSprite = platform.GetComponent<SpriteRenderer>();
        platformSprite.color = Random.value > 0.5f ? color1 : color2;

        platform.AddComponent<PlatformDestroyer>();
        platform.AddComponent<ColorChecker>();

        // Apply moving behavior based on the current score
        if (ShouldSpawnMovingPlatform())
        {
            AddMovingPlatformBehavior(platform);
        }

        // Add spikeball above the platform based on the score threshold
        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetScore();

            if (currentScore >= spikeballThreshold && Random.value < spikeballChance)
            {
                Debug.Log("Spawning Spikeball! Score: " + currentScore);
                GenerateSpikeball(y);
            }
        }
    }

    private void GenerateSpikeball(float y)
    {
        Vector3 position = new Vector3(
            Random.Range(-levelWidth / 2, levelWidth / 2),
            y + Random.Range(1.0f, 2.0f),
            0
        );
        GameObject spikeball = Instantiate(spikeballPrefab, position, Quaternion.identity);
        spikeball.AddComponent<PlatformDestroyer>();

        Debug.Log("Spikeball spawned at Y: " + y);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in platformPositions)
        {
            float horizontalDistance = Mathf.Abs(existingPosition.x - position.x);
            float verticalDistance = Mathf.Abs(existingPosition.y - position.y);

            if (horizontalDistance < minHorizontalDistance && verticalDistance < minY)
            {
                return false;
            }
        }
        return true;
    }

    private bool ShouldSpawnMovingPlatform()
    {
        if (ScoreManager.Instance != null)
        {
            int score = ScoreManager.Instance.GetScore();
            float dynamicChance = movingPlatformChance + (score / 50) * movingPlatformChanceIncrement;
            return Random.value < dynamicChance;
        }
        return false;
    }

    private void AddMovingPlatformBehavior(GameObject platform)
    {
        MovingPlatform movingPlatform = platform.AddComponent<MovingPlatform>();
        movingPlatform.SetMovementRange(levelWidth / 2, Random.Range(0.5f, 1.5f)); // Customize movement behavior
    }
}

public class PlatformDestroyer : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main.transform.position.y - 10.0f > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
