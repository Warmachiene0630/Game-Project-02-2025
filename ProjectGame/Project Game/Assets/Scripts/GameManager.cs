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
    [SerializeField] TMP_Text damageBoostPriceText;
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
    public int damageBoostPrice;
    public int speedBoostPrice;

    bool boughtSpeedBoost;
    bool isSpeedBoosted = false;
    bool boughtDamageBoost;
    bool isDamageBoosted = false;


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

    //allows oyu to change your sens in game
    public float getNewSens()
    {
        return sensSlider.normalizedValue;
    }

    //updates the player's coin count in the UI
    public void updateCoinCount(int amount)
    {
        coinCount += amount;
        coinCountText.text = coinCount.ToString("F0");
    }

    //updates prices in the Merchant Menu when menu is entered
    public void updateMerchantPrices()
    {
        healthPriceText.text = healthPrice.ToString("F0");
        damageBoostPriceText.text = damageBoostPrice.ToString("F0");
        speedBoostPriceText.text = speedBoostPrice.ToString("F0");
    }

    //pulls up Merchant Menu when store is entered
    public void enterStore()
    {
        updateMerchantPrices();
        statePause();
        menuActive = menuMerchant;
        menuActive.SetActive(true);
    }

    //if the player has enough coins, resets health and deducts coins
    //if not tells the player they don't have enough coins
    //if HP is already full, tells the player HP is full
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

    //if the player has enough coins, adds damage boost to shoot damage
    //if not tells the player they don't have enough coins
    //if a damage boost is active, tells the player a boost is already applied
    public void buyDamageBoost()
    {
        if (coinCount >= damageBoostPrice)
        {
            if (isDamageBoosted || instance.playerScript.isDamageBoosted)
            {
                resetStorePopups();
                alreadyAppliedPopup.SetActive(true);
            }
            else
            {
                resetStorePopups();
                purchaseSuccessfulPopup.SetActive(true);
                coinCount -= damageBoostPrice;
                updateCoinCount(-(damageBoostPrice));
                boughtDamageBoost = true;
                isDamageBoosted = true;
            }
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(true);
        }
    }

    //if the player has enough coins, adds speed boos to player speed
    //if not tells the player they don't have enough coins
    //if a speed boost is active, tells the player a boost is already applied
    public void buySpeedBoost()
    {
        if (coinCount >= speedBoostPrice)
        {
            if (isSpeedBoosted || instance.playerScript.isSpeedBoosted)
            {
                resetStorePopups();
                alreadyAppliedPopup.SetActive(true);
            }
            else
            {
                resetStorePopups();
                purchaseSuccessfulPopup.SetActive(true);
                coinCount -= speedBoostPrice;
                updateCoinCount(-(speedBoostPrice));
                boughtSpeedBoost = true;
                isSpeedBoosted = true;
            }
        }
        else
        {
            resetStorePopups();
            notEnoughCoinsPopup.SetActive(true);
        }
    }

    //makes sure all popups involved in the merchant store are not active
    void resetStorePopups()
    {
        notEnoughCoinsPopup.SetActive(false);
        purchaseSuccessfulPopup.SetActive(false);
        alreadyAppliedPopup.SetActive(false);
        alreadyFullPopup.SetActive(false);
    }

    //applies speed and damage boosts upon exiting the store and resets variables and popups
    public void exitStore()
    {
        if (boughtSpeedBoost)
        {
            instance.playerScript.speedBoost();
        }
        if (boughtDamageBoost)
        {
            instance.playerScript.damageBoost();
        }
        boughtSpeedBoost = false;
        boughtDamageBoost = false;
        stateUnpause();
    }
}
