using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("References")]
    public Transform player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private float startingY;
    private int currentScore;
    private int highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player != null)
        {
            startingY = player.position.y;
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceTravelled = Mathf.Max(0, player.position.y - startingY);
            currentScore = Mathf.FloorToInt(distanceTravelled);
            UpdateScoreUI(currentScore);

            if (currentScore > highScore)
            {
                highScore = currentScore;
                SaveHighScore();
                UpdateHighScoreUI();
            }
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    private void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
