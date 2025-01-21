using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the player object")]
    public Transform player;

    [Tooltip("Reference to the UI Text for the score")]
    public TextMeshProUGUI scoreText;

    private float startingY;

    /// <summary>
    /// Initialize the starting position of the player.
    /// </summary>
    private void Start()
    {
        if (player != null)
        {
            startingY = player.position.y;
        }
    }

    /// <summary>
    /// Update the score based on the player's vertical position.
    /// </summary>
    private void Update()
    {
        if (player != null)
        {
            float distanceTravelled = Mathf.Max(0, player.position.y - startingY);
            UpdateScoreUI(Mathf.FloorToInt(distanceTravelled));
        }
    }

    /// <summary>
    /// Updates the UI Text element with the current score.
    /// </summary>
    /// <param name="score">The current score to display</param>
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
