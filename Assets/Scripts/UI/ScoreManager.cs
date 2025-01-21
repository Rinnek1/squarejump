using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the player object")]
    public Transform player;

    [Tooltip("Reference to the UI Text for the score")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Reference to the UI Text for the high score")]
    public TextMeshProUGUI highScoreText;

    private float startingY;
    private int currentScore;
    private int highScore;

    /// <summary>
    /// Initialize the starting position and load the high score.
    /// </summary>
    private void Start()
    {
        if (player != null)
        {
            startingY = player.position.y;
        }

        // Load the saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    /// <summary>
    /// Update the score based on the player's vertical position.
    /// </summary>
    private void Update()
    {
        if (player != null)
        {
            float distanceTravelled = Mathf.Max(0, player.position.y - startingY);
            currentScore = Mathf.FloorToInt(distanceTravelled);
            UpdateScoreUI(currentScore);

            // Check and update the high score if necessary
            if (currentScore > highScore)
            {
                highScore = currentScore;
                SaveHighScore();
                UpdateHighScoreUI();
            }
        }
    }

    /// <summary>
    /// Updates the current score UI.
    /// </summary>
    /// <param name="score">The current score to display</param>
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    /// <summary>
    /// Updates the high score UI.
    /// </summary>
    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    /// <summary>
    /// Saves the high score using PlayerPrefs.
    /// </summary>
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save(); // Ensure the high score is saved
    }
}
