using UnityEngine;

/// <summary>
/// Controls platform generation and management in the game.
/// </summary>
public class PlatformGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject spikeballPrefab;

    [Header("Generation Settings")]
    [SerializeField] private float minY = 0.5f;
    [SerializeField] private float maxY = 2.0f;
    [SerializeField] private float levelWidth = 5.0f;
    [SerializeField] private float spikeballChance = 0.3f;

    // Platform colors
    private readonly Color color1 = new Color(0.976f, 0.380f, 0.404f); // #F96167
    private readonly Color color2 = new Color(0.976f, 0.906f, 0.584f); // #F9E795

    private float highestY;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GeneratePlatform(i * maxY);
        }
    }

    private void Update()
    {
        float playerY = Camera.main.transform.position.y;
        if (playerY + 10.0f > highestY)
        {
            GeneratePlatform(highestY + Random.Range(minY, maxY));
        }
    }

    private void GeneratePlatform(float y)
    {
        Vector3 position = new Vector3(
            Random.Range(-levelWidth / 2, levelWidth / 2),
            y,
            0
        );

        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);

        SpriteRenderer platformSprite = platform.GetComponent<SpriteRenderer>();
        platformSprite.color = Random.value > 0.5f ? color1 : color2;

        platform.AddComponent<PlatformDestroyer>();
        platform.AddComponent<ColorChecker>(); // Add the ColorChecker script for checking colors

        if (Random.value < spikeballChance)
        {
            GenerateSpikeball(y);
        }

        highestY = Mathf.Max(highestY, y);
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