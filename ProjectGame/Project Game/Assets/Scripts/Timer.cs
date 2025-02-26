using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private TMP_Text timerText;
    private bool timerRunning = true;

    void Start()
    {
        Time.timeScale = 1; // Ensure the game is running
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else if (timerRunning) // Timer reaches 0
        {
            timerRunning = false;
            timeRemaining = 0; // Prevent negative numbers
            UpdateTimerDisplay();
            GameManager.instance.youLose(); 
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
