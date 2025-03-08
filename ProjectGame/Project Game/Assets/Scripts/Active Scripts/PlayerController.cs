using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPickUp
{
    [Header("----- Animator -----")]
    [SerializeField] Animator anim;

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;

    [SerializeField] List<PlayerType> player;
    public int listPos;

    [Header("----- Stats -----")]
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

    [Header("----- Melee -----")]
    [SerializeField] meleeStats meleeWeapon;
    [SerializeField] GameObject melee;
    public Collider[] meleeCol;
    int melColPos;

    int meleeDamage;
    float meleeSpeed;
    bool meleeSelected;
    float playerSpeed;

    int jumpCount;
    int dashCount;
    public int HPCurr;

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
        HPCurr = player[listPos].HPMax;
        updatePlayerUI();
        isSlowed = false;
        if (gunList.Count() > 0) {
            meleeSelected = false;
        }
        else
        {
            meleeSelected = true;
        }

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
        controller.Move(moveDir * player[listPos].speed * Time.deltaTime);
        jump();
        controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        shootTimer += Time.deltaTime;
        speedBoostTimer -= Time.deltaTime;

        //cheks whick animation to play
        if (controller.isGrounded)
        {
            playerSpeed = controller.velocity.normalized.magnitude;
            getAnimDir();
            float animCurSpeed = anim.GetFloat("Speed");
            anim.SetBool("Melee", meleeSelected);
            anim.SetFloat("Speed", animCurSpeed);
        }

        //checks for speed boost, if there was a boost and it ended reverts speed back to original
        if(isSpeedBoosted && speedBoostTimer <= 0)
        {
            isSpeedBoosted = false;
            player[listPos].speed = player[listPos].speed / player[listPos].sprintMod;
        }

        //checks for damage boost, if there was a boost and it ended reverts shoot damage back to original
        if (isDamageBoosted && damageBoostTimer <= 0)
        {
            isDamageBoosted = false;
            shootDamage = shootDamage - damageBoostAmount;
        }
        if (meleeSelected == false)
        {
            if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
            {
                if (!GameManager.instance.isPaused)
                {
                    shoot();
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && meleeWeapon != null && shootTimer >= meleeSpeed){
                swing();
            }
        }

        selectGun();
        gunReload();
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            player[listPos].speed *= player[listPos].sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            player[listPos].speed /= player[listPos].sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(player[listPos].audSteps[Random.Range(0, player[listPos].audSteps.Length)], player[listPos].audStepsVol);

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
        HPCurr = player[listPos].HPMax;
        updatePlayerUI();
        controller.transform.position = GameManager.instance.playerSpawnPos.transform.position;
        HPCurr = player[listPos].HPMax;
        updatePlayerUI();


    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < player[listPos].jumpMax)
        {
            jumpCount++;
            playerVel.y = player[listPos].jumpSpeed;
            aud.PlayOneShot(player[listPos].audJump[Random.Range(0, player[listPos].audJump.Length)], player[listPos].audJumpVol);
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

    void swing()
    {
        isTwoHanded();
        if (meleeWeapon.twoHanded)
        {

        }
        
    }
    void isTwoHanded()
    {
        if (meleeWeapon.twoHanded == true)
        {
            melColPos = 0;
        }
        else
        {
            melColPos = 1;
        }
    }

    public void weaponColOn()
    {
        meleeCol[melColPos].enabled = true;  
    }

    public void weaponColOff()
    {
        meleeCol[melColPos].enabled = false;
    }


    public void takeDamage(int amount)
    {
        HPCurr -= amount;
        StartCoroutine(flashDamageScreen());

        updatePlayerUI();
        aud.PlayOneShot(player[listPos].audHurt[Random.Range(0, player[listPos].audHurt.Length)], player[listPos].audHurtVol);

        if (HPCurr <= 0)
        {
            GameManager.instance.youLose();
        }
    }
    public bool gainHealth(int amount)
    {
        if (HPCurr != player[listPos].HPMax) 
        {
             HPCurr += amount;

            if (HPCurr >= player[listPos].HPMax)
            {
                HPCurr = player[listPos].HPMax;
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
        GameManager.instance.playerHPBar.fillAmount = (float)HPCurr / player[listPos].HPMax;
    }

    IEnumerator dash()
    {
        player[listPos].speed *= player[listPos].sprintMod;
        yield return new WaitForSeconds(0.5f);
        player[listPos].speed /= player[listPos].sprintMod;
    }

    //used to fill HP to original
    public void fillHealth()
    {
        HPCurr = player[listPos].HPMax;
    }

    //used to check if HP is full
    public bool isHPFull()
    {
        if(HPCurr == player[listPos].HPMax)
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
        player[listPos].speed = player[listPos].speed * player[listPos].sprintMod;
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
            player[listPos].speed = player[listPos].speed / 2;
            isSlowed = true;
        }
    }

    public void normalSpeed()
    {
        player[listPos].speed = player[listPos].speed * 2;
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

        

        if (meleeSelected != true) {
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
    }
    void selectMelee()
    {
        meleeSelected = !meleeSelected;
        changeGun();
    }

    void changeGun()
    {
        if (meleeSelected == true)
        {
            gunModel.GetComponent<MeshFilter>().sharedMesh = melee.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = melee.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else {
            shootDamage = gunList[gunListPos].shootDamage;
            shootDist = gunList[gunListPos].shootDist;
            shootRate = gunList[gunListPos].shootRate;

            gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
        } 
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
        muzzleFlash.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
    }

    public void getMeleeStats(meleeStats melee)
    {
        meleeWeapon = melee;
        meleeDamage = meleeWeapon.meleeDamage;
        meleeSpeed = meleeWeapon.meleeSpeed;
    }
    void getAnimDir()
    {
        if (moveDir.x > 0)
        {
            anim.SetBool("Right", true);
        }
        else
        {
            anim.SetBool("Right", false);
        }
        if (moveDir.z > 0)
        {
            anim.SetBool("For", true);
        }
        else if(moveDir.z < 0)
        {
            anim.SetBool("For", false);
        }
        else
        {
            anim.SetBool("No For", true);
        }

    }

}
