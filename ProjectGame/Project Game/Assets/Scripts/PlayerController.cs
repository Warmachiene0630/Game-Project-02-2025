using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPickUp
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;

    [Header("----- Stats -----")]
    [Range(1, 10)] public int HP;
    [Range(3, 10)] [SerializeField] float speed;
    [Range(2, 5)] [SerializeField] float sprintMod;
    [Range(5, 20)] [SerializeField] int jumpSpeed;
    [Range(1, 3)] [SerializeField] int jumpMax;
    [Range(15, 45)] [SerializeField] int gravity;
    [Range(5, 15)] [SerializeField] float speedBoostTime;
    [Range(5, 15)] [SerializeField] float damageBoostTime;
    [Range(1, 5)] [SerializeField] int damageBoostAmount;

    [Header("----- Guns -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] Transform muzzleFlash;
    int shootDamage;
    float shootRate;
    int shootDist;
    int gunListPos;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    int jumpCount;
    int dashCount;
    int HPOrig;

    float shootTimer;
    float speedBoostTimer;
    float damageBoostTimer;

    Vector3 moveDir;

    Vector3 playerVel;

    bool isSprinting;
    bool isPlayingSteps;
    public bool isSpeedBoosted;
    public bool isDamageBoosted;

    bool isSlowed;
    float slowTimer;
    float slowDur;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
        isSlowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if disabled is false, then movement is allowed
        
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        movement();
        sprint();
        if (isSlowed == true) {
            checkSlow();
        }
    } 

    void movement()
    {
        if (controller.isGrounded)
        {
            if (moveDir.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
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

        //checks for speed boost, if there was a boost and it ended reverts speed back to original
        if(isSpeedBoosted && speedBoostTimer <= 0)
        {
            isSpeedBoosted = false;
            speed = speed / sprintMod;
        }

        //checks for damage boost, if there was a boost and it ended reverts shoot damage back to original
        if (isDamageBoosted && damageBoostTimer <= 0)
        {
            isDamageBoosted = false;
            shootDamage = shootDamage - damageBoostAmount;
        }

        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
        {
            if (!GameManager.instance.isPaused)
            {
                shoot();
            }
            

        }

        selectGun();
        gunReload();
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

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!isSprinting)
        {
            if (!isSlowed)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.7f);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        isPlayingSteps = false;
    }
    public void spawnPlayer()
    {
        HP = HPOrig;
        updatePlayerUI();
        controller.transform.position = GameManager.instance.playerSpawnPos.transform.position;
        HP = HPOrig;
        updatePlayerUI();


    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
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
        gunList[gunListPos].ammoCur--;
        aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootVol);

        StartCoroutine(flashMuzzle());

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            //Debug.Log(hit.collider.name);

            Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

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
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);

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

    //used to fill HP to original
    public void fillHealth()
    {
        HP = HPOrig;
    }

    //used to check if HP is full
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

    //adds speed boost to player using sprint mod and starts countdown
    public void speedBoost()
    {
        isSpeedBoosted = true;
        speed = speed * sprintMod;
        speedBoostTimer = speedBoostTime;
    }

    //adds damage boost to player using damage boost amount variable and starts countdown
    public void damageBoost()
    {
        isDamageBoosted = true;
        shootDamage += damageBoostAmount;
        damageBoostTimer = damageBoostTime;
    }

    private void checkSlow()
    {
        slowTimer += Time.deltaTime;
        if (slowTimer >= slowDur)
        {
            normalSpeed();
        }
    }
    public void slowSpeed(int slow)
    {
        if (isSlowed != true)
        {
            slowDur = slow;
            speed = speed / 2;
            isSlowed = true;
        }
    }

    public void normalSpeed()
    {
        speed = speed * 2;
        isSlowed = false;
        slowDur = 0;
        slowTimer = 0;
    }

    public void getGunStats(GunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;

        changeGun();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        {
            gunListPos++;
            changeGun();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }
    }

    void changeGun()
    {

        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void gunReload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
            aud.PlayOneShot(gunList[gunListPos].reloadSound[Random.Range(0, gunList[gunListPos].reloadSound.Length)], gunList[gunListPos].reloadVol);
        }

    }

    IEnumerator flashMuzzle()
    {
        //muzzleFlash.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
    }
}
