using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject platformPrefab;
    public float platformWidth = 2.5f;
    public float minY = 0.5f;
    public float maxY = 2.0f;
    public float levelWidth = 5.0f;

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
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        platform.AddComponent<PlatformDestroyer>();
        highestY = Mathf.Max(highestY, y);
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