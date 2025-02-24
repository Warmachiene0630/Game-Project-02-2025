using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPickUp
{

    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] float speedBoostTime;

    int jumpCount;
    int dashCount;
    int HPOrig;

    float shootTimer;
    float speedBoostTimer;

    Vector3 moveDir;

    Vector3 playerVel;

    bool isSprinting;
    public bool isBoosted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        //if disabled is false, then movement is allowed
        
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

            movement();
            sprint();
    } 

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            dashCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);
        jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        shootTimer += Time.deltaTime;
        speedBoostTimer -= Time.deltaTime;

        if(isBoosted && speedBoostTimer <= 0)
        {
            isBoosted = false;
            speed = speed / sprintMod;
        }

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            shoot();
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
        else if (Input.GetButtonDown("Jump") && dashCount == 0)
        {
            dashCount++;
            StartCoroutine(dash());
        }    
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamageScreen());

        updatePlayerUI();

        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }
    public bool gainHealth(int amount)
    {
        if (HP != HPOrig) 
        {
            HP += amount;

            if (HP >= HPOrig)
            {
                HP = HPOrig;
            }
            
            StartCoroutine(flashHealthScreen());

            updatePlayerUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator flashDamageScreen()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    IEnumerator flashHealthScreen()
    {
        GameManager.instance.playerHealthScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerHealthScreen.SetActive(false);
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator dash()
    {
        speed *= sprintMod;
        yield return new WaitForSeconds(0.5f);
        speed /= sprintMod;
    }

    public void fillHealth()
    {
        HP = HPOrig;
    }

    public bool isHPFull()
    {
        if(HP == HPOrig)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void speedBoost()
    {
        isBoosted = true;
        speed = speed * sprintMod;
        speedBoostTimer = speedBoostTime;
    }
}
