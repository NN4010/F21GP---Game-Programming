using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int carriedSnacks = 0;
    public int requiredSnacks = 10;

    public TMP_Text carryText;
    public TMP_Text timerText;
    public TMP_Text messageText;

    public float timeLimit = 120f;
    private float timeRemaining;
    private bool gameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeRemaining = timeLimit;
        UpdateCarryUI();
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining);
        }

        if (timeRemaining <= 0)
        {
            LoseGame();
        }
    }

    public void AddSnack(int amount)
    {
        carriedSnacks += amount;
        UpdateCarryUI();
    }

    void UpdateCarryUI()
    {
        carryText.text = "Snacks: " + carriedSnacks + "/" + requiredSnacks;
    }

    public void CheckWin()
    {
        if (carriedSnacks >= requiredSnacks)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        gameOver = true;
        messageText.text = "You Win!";
    }

    void LoseGame()
    {
        gameOver = true;
        messageText.text = "You Lose!";
    }
}
