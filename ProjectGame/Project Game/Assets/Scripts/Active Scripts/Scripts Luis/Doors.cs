using UnityEngine;

public class Doors : MonoBehaviour
{
    public int keyCount;
    [SerializeField] Key[] keys;
    int sceneChoice;

    private bool open;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyCount = keys.Length;
        open = false;
        sceneChoice = GameManager.sceneList.length;
    }

    // Update is called once per frame
    void Update()
    {
        keyCount = keys.Length;
        if (keyCount == 0)
        {
            open = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (open == true)
            {
                loadNextLevel(GameManager.sceneList[sceneChoice]);
            }
            else
            {
                missingKeys();
            }
        }
    }

    void missingKeys()
    {

    }

    void loadNextLevel(string nextScene)
    {
        LevelManager.Instance.loadScene(nextScene);
    }
}
