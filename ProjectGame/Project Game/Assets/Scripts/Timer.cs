using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit = 60f; 
    [SerializeField] private GameObject youLoseScreen; 
    private float currentTime;
    private bool timerRunning = true;

    public TMP_Text TimerText; 
    public TMP_Text LoseTimeText; 

    void Start()
    {
        currentTime = timeLimit; // Initialize timer
        timerRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerRunning) return; 

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            timerRunning = false;

            
            if (GameManager.instance != null && GameManager.instance.youLoseScreen != null)
            {
                GameManager.instance.youLoseScreen.SetActive(true);
            }

          
            if (LoseTimeText != null)
            {
                LoseTimeText.text = $"Time's Up! You lasted {timeLimit} seconds";
            }

            Debug.Log("Time's up! Player loses.");
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        if (TimerText != null)
        {
            TimerText.text = Mathf.Ceil(currentTime).ToString(); 
        }
    }
}