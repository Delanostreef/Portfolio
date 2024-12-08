using BNG;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneTarget : MonoBehaviour
{
    public bool isStarted;
    public bool hasEnded;
    public float timeLeft = 60;
    public TextMeshProUGUI timerCondition;

    [SerializeField] private GameObject Targets;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject conditionBlock;

    [SerializeField] private GameObject ToDisableTargets;
    [SerializeField] private GameObject DisableButton;
    public void EnterOneTarget()
    {
        DisableButton.SetActive(false);
        hasEnded = false;
        startButton.SetActive(true);
        conditionBlock.SetActive(true);
        Targets.SetActive(true);
        timeLeft = 60;
        timerCondition.text = "Time left: 60";
        Score.Instance.currentScore = 0;
        Score.Instance.scoreBoard.text = "Score: 0";
        Score.Instance.scoreLock = true;
        Score.Instance.SetGameMode(Score.GameMode.OneTarget);
        Score.Instance.DisplayHighScore(Score.GameMode.OneTarget);
        ToDisableTargets.SetActive(false);
        isStarted = false;
        timerCondition.text = "Time left: " + timeLeft.ToString("F2");
    }

    public void StartOneTarget()
    {
        isStarted = true;
        Score.Instance.scoreLock = false;
        Debug.Log("started one target");
    }

    private void Update()
    {
        if (isStarted & timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerCondition.text = "Time left: " + timeLeft.ToString("F2");
        }

        if (isStarted == true & timeLeft <= 0)
        {
            timeLeft = 0;
            hasEnded = true;
            DisableTargets();
            Score.Instance.scoreLock = true;
            Score.Instance.UpdateHighScore(Score.GameMode.OneTarget, Score.Instance.currentScore);
            Score.Instance.DisplayHighScore(Score.GameMode.OneTarget);
            Debug.Log("Scorelock activated");
        }
    }

    public void StopCount()
    {
        isStarted = false;
    }

    public void DisableTargets()
    {
        Targets.SetActive(false);
        isStarted = false;
    }
}
