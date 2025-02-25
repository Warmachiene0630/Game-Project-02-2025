using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }

    public void confirmSens()
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        CameraController camScript = cam.GetComponent<CameraController>();
        int sensOrig = camScript.getSens();
        float sensMultiplier = GameManager.instance.getNewSens();
        if(sensMultiplier > 0)
        {
            int newSens = (int)(sensMultiplier * 1000);
            camScript.setSens(newSens);
        }
        else
        {
            camScript.setSens(sensOrig);
        }
        GameManager.instance.stateUnpause();
    }

    public void backPause()
    {
        GameManager.instance.statePause();
    }

    public void backSett()
    {
        GameManager.instance.settings();
    }
    public void openSens()
    {
        GameManager.instance.sensitivity();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        GameManager.instance.stateUnpause();
    }


    public void buyHealth()
    {
        GameManager.instance.buyHealth();
    }


    public void buyDamageBoost()
    {
        GameManager.instance.buyDamageBoost();
    }

    public void buySpeedBoost()
    {
        GameManager.instance.buySpeedBoost();
    }

    public void leaveStore()
    {
        GameManager.instance.exitStore();
    }
}

