using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float startTime = 60f; // Set this per level in the Inspector
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject youLoseScreen; // Assign the "You Lose" UI panel in Inspector

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = startTime; // Initialize the timer
        UpdateTimerUI();
        youLoseScreen.SetActive(false); // Hide "You Lose" screen at start
    }

    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            if (timeRemaining <= 0)
            {
                EndTimer();
            }
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndTimer()
    {
        timeRemaining = 0;
        isRunning = false;
        Time.timeScale = 0f; // Pause the game
        youLoseScreen.SetActive(true); // "You Lose" screen
    }
}