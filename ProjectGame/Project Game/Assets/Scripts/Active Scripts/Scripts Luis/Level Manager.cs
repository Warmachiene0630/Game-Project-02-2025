using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;

    public static LevelManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void loadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        //playerHudOff();

        loadingScreen.SetActive(true);

        while (scene.progress < 0.9f)
        {
            await Task.Delay(80);

            loadingBar.fillAmount = scene.progress;

        }

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        
        loadingScreen.SetActive(false);
        //playerHudOn();
    }
}
