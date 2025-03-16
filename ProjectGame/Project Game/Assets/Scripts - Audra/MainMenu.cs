using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
