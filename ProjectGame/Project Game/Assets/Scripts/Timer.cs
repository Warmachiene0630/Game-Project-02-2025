using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit = 60f; // Editable per level in Unity
    private float currentTime;
    private bool timerRunning = true;

    public Text timerText; // Assign in the Unity Inspector

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerRunning = false;

                // Use GameManager to trigger the "You Lose" screen
                GameManager.instance.youLose();
                Debug.Log("Time's up! Player loses.");
            }

            if (timerText != null)
            {
                // Display countdown timer, rounded down to an integer
                timerText.text = Mathf.Floor(currentTime).ToString();
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = timeLimit;
        timerRunning = true;
    }
}