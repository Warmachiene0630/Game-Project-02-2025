using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public static GameManager instance;
    public GameObject player;
    public PlayerController playerScript;
    [SerializeField] AudioSource aud;

    [Range(50, 500)] public int healthPrice;
    [Range(50, 500)] public int damageBoostPrice;
    [Range(50, 500)] public int speedBoostPrice;

    [Header("----- Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuSens;
    [SerializeField] GameObject menuMerchant;
    public bool isPaused;

    [Header("----- UI -----")]
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text coinCountText;
    [SerializeField] TMP_Text healthPriceText;
    [SerializeField] TMP_Text damageBoostPriceText;
    [SerializeField] TMP_Text speedBoostPriceText;
    [SerializeField] Slider sensSlider;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
    public GameObject playerHealthScreen;

    [Header("----- Popups -----")]
    public GameObject teleportPopup;
    public GameObject merchantPopup;
    public GameObject notEnoughCoinsPopup;
    public GameObject purchaseSuccessfulPopup;
    public GameObject alreadyFullPopup;
    public GameObject alreadyAppliedPopup;

    [Header("----- Stats -----")]
    private int goalCount;
    private int coinCount;

    [Header("----- Boosts -----")]
    bool boughtSpeedBoost;
    bool isSpeedBoosted = false;
    bool boughtDamageBoost;
    bool isDamageBoosted = false;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip backgroundMusic;
    [Range(0, 1)][SerializeField] float musicVol;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure Main Menu is the first scene to load
        if (SceneManager.GetActiveScene().name != "MainMenu - Caleb")
        {
            SceneManager.LoadScene("MainMenu - Caleb");
            return; // Prevent further execution until Main Menu is loaded
        }

        // Ensure UI and Cursor Settings
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Try to find player only if not in Main Menu
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        }

        // Play Background Music if Audio Source is Assigned
        if (aud != null && backgroundMusic != null)
        {
            aud.PlayOneShot(backgroundMusic, musicVol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
        resetStorePopups();
    }

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");
        if (goalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void settings()
    {
        if (menuActive != null)
            menuActive.SetActive(false);
        statePause();
        menuActive = menuSettings;
        menuActive.SetActive(true);
    }

    public void sensitivity()
    {
        if (menuActive != null)
            menuActive.SetActive(false);
        statePause();
        menuActive = menuSens;
        menuActive.SetActive(true);
    }

    public float getNewSens()
    {
        return sensSlider.normalizedValue;
    }

    public void updateCoinCount(int amount)
    {
        coinCount += amount;
        coinCountText.text = coinCount.ToString("F0");
    }

    public void updateMerchantPrices()
    {
        healthPriceText.text = healthPrice.ToString("F0");
        damageBoostPriceText.text = damageBoostPrice.ToString("F0");
        speedBoostPriceText.text = speedBoostPrice.ToString("F0");
    }

    public void enterStore()
    {
        updateMerchantPrices();
        statePause();
        menuActive = menuMerchant;
        menuActive.SetActive(true);
    }

    public void buyHealth()
    {
        if (playerScript == null) return;
        if (coinCount >= healthPrice && !playerScript.isHPFull())
        {
            resetStorePopups();
            purchaseSuccessfulPopup.SetActive(true);
            playerScript.fillHealth();
            updateCoinCount(-healthPrice);
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(playerScript.isHPFull() ? alreadyFullPopup : notEnoughCoinsPopup);
        }
    }

    public void buyDamageBoost()
    {
        if (coinCount >= damageBoostPrice && !isDamageBoosted)
        {
            resetStorePopups();
            purchaseSuccessfulPopup.SetActive(true);
            updateCoinCount(-damageBoostPrice);
            isDamageBoosted = true;
        }
        else
        {
            resetStorePopups();
            alreadyAppliedPopup.SetActive(true);
        }
    }

    public void buySpeedBoost()
    {
        if (coinCount >= speedBoostPrice && !isSpeedBoosted)
        {
            resetStorePopups();
            purchaseSuccessfulPopup.SetActive(true);
            updateCoinCount(-speedBoostPrice);
            isSpeedBoosted = true;
        }
        else
        {
            resetStorePopups();
            alreadyAppliedPopup.SetActive(true);
        }
    }

    void resetStorePopups()
    {
        notEnoughCoinsPopup.SetActive(false);
        purchaseSuccessfulPopup.SetActive(false);
        alreadyAppliedPopup.SetActive(false);
        alreadyFullPopup.SetActive(false);
    }

    public void exitStore()
    {
        stateUnpause();
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("CalebScene");
    }
}