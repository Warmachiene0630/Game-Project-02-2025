using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    private int skipCount;
    [SerializeField] GameObject typePanel;
    [SerializeField] GameObject fullPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skipCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Skip") && skipCount > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetButtonDown("Skip"))
        {
            skipCount++;
            fullPanel.SetActive(true);
            typePanel.SetActive(false);
        }
        
    }
}
