using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame()
    {
        Application.Quit();
    }
    public void confirmSens()
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        CameraController camScript = cam.GetComponent<CameraController>();
        int sensOrig = camScript.getSens();
        float sensMultiplier = GameManager.instance.getNewSens();
        if (sensMultiplier > 0)
        {
            int newSens = (int)(sensMultiplier * 1000);
            camScript.setSens(newSens);
        }
        else
        {
            camScript.setSens(sensOrig);
        }
    }

}
