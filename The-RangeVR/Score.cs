using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance;
    public bool scoreLock;

    public bool isStarted;

    public GameMode currentGameMode;

    [SerializeField] public int currentScore;
    [SerializeField] public TextMeshProUGUI scoreBoard;

    public TextMeshProUGUI highScoreText;

    public enum GameMode
    {
        OneTarget,
        TimeAttack
    }

    public GameMode gameMode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentScore = 0;
        scoreBoard.text = "Score: " + currentScore;
    }

    public void gainScore(int Scored)
    {
        if (scoreLock == false)
        {
            currentScore = currentScore + Scored;
            scoreBoard.text = "Score: " + currentScore;
        }

    }

    public void SetGameMode(GameMode mode)
    {
        currentGameMode = mode; // Set the current game mode
    }

    public void UpdateHighScore(GameMode mode, int currentScore)
    {
        string key = "HighScore_" + mode.ToString();

        // Check if current score is higher than saved score
        if (currentScore > PlayerPrefs.GetInt(key, 0))
        {
            // Update high score
            PlayerPrefs.SetInt(key, currentScore);
            PlayerPrefs.Save(); // Save changes
        }
    }

    public void DisplayHighScore(GameMode mode)
    {
        string key = "HighScore_" + mode.ToString();
        int highScore = PlayerPrefs.GetInt(key, 0); // Get high score for this mode

        // Display high score in UI
        highScoreText.text = mode + " High Score: " + highScore.ToString();
    }
}
