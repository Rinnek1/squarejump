using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject platformPrefab1;
    public GameObject platformPrefab2;
    public GameObject spikeballPrefab; // Spikeball prefab
    public float platformWidth = 2.5f;
    public float minY = 0.5f;
    public float maxY = 2.0f;
    public float levelWidth = 5.0f;
    public float spikeballChance = 0.3f; // Chance to spawn a spikeball (0 to 1)

    private float highestY;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GeneratePlatform(i * maxY);
        }
    }

    void Update()
    {
        float playerY = Camera.main.transform.position.y;

        if (playerY + 10.0f > highestY)
        {
            GeneratePlatform(highestY + Random.Range(minY, maxY));
        }
    }

    void GeneratePlatform(float y)
    {
        float x = Random.Range(-levelWidth / 2, levelWidth / 2);
        Vector3 position = new Vector3(x, y, 0);

        GameObject selectedPrefab = Random.value > 0.5f ? platformPrefab1 : platformPrefab2;
        GameObject platform = Instantiate(selectedPrefab, position, Quaternion.identity);
        platform.AddComponent<PlatformDestroyer>();

        // Randomly generate a spikeball near the platform
        if (Random.value < spikeballChance)
        {
            GenerateSpikeball(y);
        }

        highestY = Mathf.Max(highestY, y);
    }

    void GenerateSpikeball(float y)
    {
        float x = Random.Range(-levelWidth / 2, levelWidth / 2);
        Vector3 position = new Vector3(x, y + Random.Range(1.0f, 2.0f), 0); // Slightly above the platform
        GameObject spikeball = Instantiate(spikeballPrefab, position, Quaternion.identity);
        spikeball.AddComponent<PlatformDestroyer>();
    }
}

public class PlatformDestroyer : MonoBehaviour
{
    void Update()
    {
        if (Camera.main.transform.position.y - 10.0f > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}