using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuSens;
    [SerializeField] GameObject menuMerchant;
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text coinCountText;
    [SerializeField] TMP_Text healthPriceText;
    [SerializeField] TMP_Text ammoPriceText;
    [SerializeField] TMP_Text speedBoostPriceText;
    [SerializeField] Slider sensSlider;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
    public GameObject playerHealthScreen;
    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;

    public GameObject teleportPopup;
    public GameObject merchantPopup;
    public GameObject notEnoughCoinsPopup;
    public GameObject purchaseSuccessfulPopup;
    public GameObject alreadyFullPopup;
    public GameObject alreadyAppliedPopup;

    private int goalCount;
    public int coinCount;
    public int healthPrice;
    public int ammoPrice;
    public int speedBoostPrice;

    bool boughtBoost;
    bool isBoosted = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
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
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        resetStorePopups();
    }

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        //goalCountText.text = goalCount.ToString("F0");
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
        menuActive.SetActive(false);
        statePause();
        menuActive = menuSettings;
        menuActive.SetActive(true);
    }

    public void sensitivity()
    {
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
        ammoPriceText.text = ammoPrice.ToString("F0");
        speedBoostPriceText.text = speedBoostPrice.ToString("F0");
    }

    public void enterStore()
    {
        //merchantPopup.SetActive(false);
        updateMerchantPrices();
        statePause();
        menuActive = menuMerchant;
        menuActive.SetActive(true);
    }

    //popups all remain on screen, even after leaving menu
    public void buyHealth()
    {
        bool isHPFull = playerScript.isHPFull();
        if (coinCount >= healthPrice)
        {
            if (isHPFull)
            {
                resetStorePopups();
                alreadyFullPopup.SetActive(true);
            }
            else
            {
                resetStorePopups();
                purchaseSuccessfulPopup.SetActive(true);
                playerScript.fillHealth();
                playerScript.updatePlayerUI();
                updateCoinCount(-(healthPrice));
            }
            
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(true);
        }
    }


    public void buyAmmo()
    {
        if (coinCount >= ammoPrice)
        {
            resetStorePopups();
            purchaseSuccessfulPopup.SetActive(true);
            //fill ammo
            coinCount -= ammoPrice;
            playerScript.updatePlayerUI();
            updateCoinCount(-(ammoPrice));
            
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(true);
        }
    }

    public void buySpeedBoost()
    {
        if (coinCount >= speedBoostPrice)
        {
            if (isBoosted || instance.playerScript.isBoosted)
            {
                resetStorePopups();
                alreadyAppliedPopup.SetActive(true);
            }
            else
            {
                resetStorePopups();
                purchaseSuccessfulPopup.SetActive(true);
                //double speed (need new public method with timer/countdown in player contorller)
                coinCount -= speedBoostPrice;
                updateCoinCount(-(speedBoostPrice));
                boughtBoost = true;
                isBoosted = true;
            }
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(true);
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
        if (boughtBoost)
        {
            instance.playerScript.speedBoost();
        }
        boughtBoost = false;
        stateUnpause();
    }
}
