using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private float screenWidth;

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
    }

    void Update()
    {
        Vector3 position = transform.position;

        if (position.x < -screenWidth / 2)
        {
            position.x = screenWidth / 2;
        }
        else if (position.x > screenWidth / 2)
        {
            position.x = -screenWidth / 2;
        }

        transform.position = position;
    }
}
