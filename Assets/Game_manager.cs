using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Snacks & Rally Points")]
    public int carriedSnacks = 0;
    public int requiredSnacks = 10;
    public int rallyPoints = 0;

    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;
    public GameObject[] lifeIcons; // heart images in UI

    [Header("Timer")]
    public float timeLimit = 120f;
    private float timeRemaining;

    [Header("UI")]
    public TMP_Text carryText;
    public TMP_Text timerText;
    public TMP_Text messageText;
    public TMP_Text startText;
    public TMP_Text floatingRallyText; // assign FloatingRallyText from FloatingTextCanvas here


    private bool gameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentLives = maxLives;
        timeRemaining = timeLimit;

        UpdateCarryUI();
        UpdateLivesUI();
        StartCoroutine(ShowStartMessage());
    }

    void Update()
    {
        if (gameOver) return;

        // Timer countdown
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

    IEnumerator ShowStartMessage()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            startText.gameObject.SetActive(false);
        }
    }

    // Called by RallyPoint script
    public void AddRallyPoint(int amount)
{
    rallyPoints += amount;

    // Show floating text above player
    if (floatingRallyText != null)
    {
        StartCoroutine(FloatingTextRoutine(amount));
    }
}

    // Called by snack pickup
    public void AddSnack(int amount)
    {
        carriedSnacks += amount;
        UpdateCarryUI();

        if (carriedSnacks >= requiredSnacks)
        {
            WinGame();
        }
    }

    void UpdateCarryUI()
    {
        if (carryText != null)
            carryText.text = "Snacks: " + carriedSnacks + "/" + requiredSnacks;
    }

    // Called by enemy hitting player
    public void TakeDamage()
    {
        if (gameOver) return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            LoseGame();
        }
    }

    void UpdateLivesUI()
    {
        if (lifeIcons == null) return;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }

    void WinGame()
    {
        gameOver = true;
        if (messageText != null)
            messageText.text = "You Win!";
    }

    void LoseGame()
    {
        gameOver = true;
        if (messageText != null)
            messageText.text = "You Lose!";
    }

    // -------- Floating Rally Text Methods --------
    public void ShowFloatingRallyText(int amount)
    {
        if (floatingRallyText != null)
            StartCoroutine(FloatingTextRoutine(amount));
    }

    private IEnumerator FloatingTextRoutine(int amount)
{
    floatingRallyText.text = "+" + amount;
    floatingRallyText.gameObject.SetActive(true);

    Vector3 startPos = floatingRallyText.transform.localPosition;
    Vector3 endPos = startPos + Vector3.up * 0.5f; // floats upward

    float duration = 1f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        floatingRallyText.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
        elapsed += Time.deltaTime;
        yield return null;
    }

    floatingRallyText.transform.localPosition = startPos; // reset position
    floatingRallyText.gameObject.SetActive(false);       // hide text
}
}
