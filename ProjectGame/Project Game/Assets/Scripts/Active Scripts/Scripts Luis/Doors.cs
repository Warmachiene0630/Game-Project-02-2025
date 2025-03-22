using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    public int keyCount;
    int sceneChoice;

    private bool open;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyCount = GameManager.instance.goalCount;
        open = false;
        sceneChoice = Random.Range(0, GameManager.instance.sceneList.Length);
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (open == true)
            {
                GameManager.instance.loadNextScene();
            }
            else
            {
                missingKeys();
            }
        }
    }

    IEnumerator missingKeys()
    {

        GameManager.instance.doorPopUp.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.doorPopUp.SetActive(false);
    
    }

    void loadNextLevel(string nextScene)
    {
        LevelManager.Instance.loadScene(nextScene);
    }
}
