using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    private int enemyCount = 0; // Track enemies

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Assign the instance
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void updateGameGoal(int change)
    {
        enemyCount += change;
        Debug.Log("Enemy Count: " + enemyCount);
    }
}
