using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamage, IPickUp
{
    [Header("----- Animator -----")]
    [SerializeField] Animator anim;

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject playerModel;
    [SerializeField] List<PlayerType> player;
    public int listPos;

    [Header("----- Jetpack -----")]
    [Range(0, 1)][SerializeField] float holdTime;
    [Range(10, 20)][SerializeField] float flightSpeed;
    [Range(5, 20)][SerializeField] int fuelMax;
    [SerializeField] bool hasJetpack;
    bool jumpPressed = false;
    float timeHeld = 0;
    float fuel;
    int fuelZeroCount = 0;

    [Header("----- Stats -----")]
    [Range(15, 45)][SerializeField] int gravity;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [Range(5, 15)][SerializeField] float speedBoostTime;
    [Range(5, 15)][SerializeField] float damageBoostTime;
    [Range(1, 5)][SerializeField] int damageBoostAmount;

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
    [SerializeField] AudioClip[] audFly;
    [Range(0, 1)][SerializeField] float audFlyVol;
    [SerializeField] AudioClip[] audIce;
    [Range(0, 1)][SerializeField] float audIceVol;

    [Header("----- Melee -----")]
    [SerializeField] meleeStats meleeWeapon;
    [SerializeField] GameObject melee;
    public Collider meleeCol;

    int meleeDamage;
    float meleeTimer;
    float meleeSpeed;
    float playerSpeed;

    float speed;
    int jumpCount;
    int dashCount;
    public int HPCurr;
    int lifeCount;
    int gravityOrig;

    float shootTimer;
    float speedBoostTimer;
    float damageBoostTimer;

    Vector3 moveDir;

    Vector3 playerVel;


    bool isSprinting;
    bool isDashing;
    bool isFlying;
    bool isPlayingSteps;
    public bool isSpeedBoosted;
    public bool isDamageBoosted;

    bool isSlowed;
    float slowTimer;
    float slowDur;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerModel.GetComponent<SkinnedMeshRenderer>().sharedMesh = player[listPos].model.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        if (player[listPos].firstScene == true)
        {
            HPCurr = player[listPos].HPMax;
            lifeCount = 3;
            fuel = fuelMax;
            meleeWeapon = player[listPos].assignedWeapon;
            getGunStats(player[listPos].assignedGun);
        }
        else
        {
            lifeCount = player[listPos].livesLeft;
            HPCurr = player[listPos].healthRemaining;
            GameManager.instance.updateCoinCount(player[listPos].totalGold);
            gunList = player[listPos].guns;

        }
        //HPOrig = HP;

        
        gravityOrig = gravity;
        updatePlayerUI();
        isSlowed = false;
        speed = player[listPos].speedBase;
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
            if (isDashing == false)
            {
                dashCount = 0;
            }
            isFlying = false;
            jumpCount = 0;
            dashCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);
        //controller.Move(moveDir * 4 * Time.deltaTime);
        controller.Move(playerVel * Time.deltaTime);
        if (controller.isGrounded == false)
        {
            playerVel.y -= gravity * Time.deltaTime;
        }
        else
        {
            playerVel.y = 0;
        }

        shootTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;
        speedBoostTimer -= Time.deltaTime;

        //cheks whick animation to play
        playerSpeed = controller.velocity.magnitude;
        getAnimDir();
        jump();
        playerAbility();

        //checks for speed boost, if there was a boost and it ended reverts speed back to original
        if (isSpeedBoosted && speedBoostTimer <= 0)
        {
            isSpeedBoosted = false;
            speed = speed / player[listPos].sprintMod;
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
        if (Input.GetButton("Melee") && meleeWeapon != null && meleeTimer >= meleeSpeed) {
            swing();
        }

        selectGun();
        gunReload();
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= player[listPos].sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            speed /= player[listPos].sprintMod;
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
            jumpCount += 1;
            playerVel.y = player[listPos].jumpSpeed;
            aud.PlayOneShot(player[listPos].audJump[Random.Range(0, player[listPos].audJump.Length)], player[listPos].audJumpVol);
        }
    }


    void shoot()
    {
        shootTimer = 0;
        gunList[gunListPos].ammoCur--;
        aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootVol);

        StartCoroutine(flashMuzzle());
        anim.SetTrigger("Shoot");

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
        shootTimer = 0;
        gunModel.GetComponent<MeshFilter>().sharedMesh = null;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = null;

        melee.GetComponent<MeshFilter>().sharedMesh = meleeWeapon.model.GetComponent<MeshFilter>().sharedMesh;
        melee.GetComponent<MeshRenderer>().sharedMaterial = meleeWeapon.model.GetComponent<MeshRenderer>().sharedMaterial;

        anim.SetTrigger("Swing 1");

        weaponColOff();
        changeGun();



    }

    public void weaponColOn()
    {
        meleeCol.enabled = true;
    }

    public void weaponColOff()
    {
        meleeCol.enabled = false;
        melee.GetComponent<MeshFilter>().sharedMesh = null;
        melee.GetComponent<MeshRenderer>().sharedMaterial = null;
    }


    public void takeDamage(int amount)
    {
        HPCurr -= amount;
        StartCoroutine(flashDamageScreen());

        updatePlayerUI();
        aud.PlayOneShot(player[listPos].audHurt[Random.Range(0, player[listPos].audHurt.Length)], player[listPos].audHurtVol);

        if (HPCurr <= 0)
        {
            lifeCount = lifeCount - 1;
            HPCurr = player[listPos].HPMax;
            updatePlayerUI();
            spawnPlayer();
        }

        if (lifeCount <= 0)
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
        GameManager.instance.playerFuelBar.fillAmount = (float)fuel / fuelMax;
        if (lifeCount == 2)
        {
            GameManager.instance.life3.fillAmount = 0;
            GameManager.instance.life2.fillAmount = 100;
            GameManager.instance.life1.fillAmount = 100;
        }
        if (lifeCount == 1)
        {
            GameManager.instance.life2.fillAmount = 0;
            GameManager.instance.life3.fillAmount = 0;
            GameManager.instance.life1.fillAmount = 100;
        }
        if (lifeCount == 0)
        {
            GameManager.instance.life1.fillAmount = 0;
            GameManager.instance.life2.fillAmount = 0;
            GameManager.instance.life3.fillAmount = 0;
        }
    }

    void dash()
    {
        if (Input.GetButtonDown("Dash") && dashCount < 1)
        {
            dashCount++;
            aud.PlayOneShot(player[listPos].audJump[Random.Range(0, player[listPos].audJump.Length)], player[listPos].audJumpVol);
            StartCoroutine(dashCoroutine());
        }
    }
    IEnumerator dashCoroutine()
    {
        float startTime = Time.time;
        Vector3 dashDir = Camera.main.transform.forward;
        dashDir.y = 0.5f;
        GameManager.instance.playerDashScreen.SetActive(true);
        isDashing = true;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            yield return null;
        }
        GameManager.instance.playerDashScreen.SetActive(false);
        isDashing = false;
    }

    //used to fill HP to original
    public void fillHealth()
    {
        HPCurr = player[listPos].HPMax;
    }

    //used to check if HP is full
    public bool isHPFull()
    {
        if (HPCurr == player[listPos].HPMax)
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
        speed = speed * player[listPos].sprintMod;
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
        if (gunList.Count > 0) {
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
        anim.SetFloat("Right", Input.GetAxis("Horizontal"));
        anim.SetFloat("For", Input.GetAxis("Vertical"));
    }

    void fly()
    {
        if (Input.GetButtonDown("Ability"))
        {
            jumpPressed = true;
            if (isFlying)
            {
                timeHeld = holdTime;
            }
        }
        else if (Input.GetButtonUp("Ability"))
        {
            jumpPressed = false;
            timeHeld = 0;
            aud.Stop();
        }
        else if (fuel <= 0 && fuelZeroCount == 0)
        {
            jumpPressed = false;
            timeHeld = 0;
            aud.Stop();
            fuelZeroCount++;
        }
        else if (!jumpPressed)
        {
            timeHeld = 0;
        }
        timeHeld += Time.deltaTime;
        if (jumpPressed == true && timeHeld >= holdTime && fuel > 0)
        {
            aud.PlayOneShot(audFly[0], audFlyVol);
            playerVel.y = 1 * flightSpeed;
            isFlying = true;
            fuel -= Time.deltaTime * (flightSpeed / 2);
            updatePlayerUI();
        }
    }
    public bool gainFuel(float amount)
    {
        bool fuelGained;
        if (fuel < fuelMax)
        {
            fuel += amount;
            if (fuel > fuelMax)
            {
                fuel = fuelMax;
            }
            fuelGained = true;
            fuelZeroCount = 0;
        }
        else
        {
            fuelGained = false;
        }
        updatePlayerUI();
        return fuelGained;
    }

    void increaseSpeed(int mod)
    {
        speed *= mod;
        StartCoroutine(iceAud(true));
    }

    IEnumerator iceAud(bool on)
    {
        if (on)
        {
            yield return new WaitForSeconds(0.1f);
            //   aud.PlayOneShot(audIce[Random.Range(0, audIce.Length)], audIceVol);
        }
        else
        {
            aud.Stop();
            yield return null;
        }
    }

    void decreaseSpeed(int mod)
    {
        speed /= mod;
        StartCoroutine(iceAud(false));
    }

    void changeGravity(int newGrav)
    {
        gravity = newGrav;
        speed /= 2;
        player[listPos].jumpSpeed /= 2;
        flightSpeed /= 2;
    }

    void revertGravity()
    {
        gravity = gravityOrig;
        speed *= 2;
        player[listPos].jumpSpeed *= 2;
        flightSpeed *= 2;
    }

    public void assignStats()
    {
        player[listPos].healthRemaining = HPCurr;
        player[listPos].totalGold = GameManager.instance.getCoins();
        player[listPos].livesLeft = lifeCount;
        player[listPos].firstScene = false;
        player[listPos].remainigFuel = fuel;
        player[listPos].guns = gunList;
    }

    void playerAbility()
    {
        if (listPos == 0) {
            dash();
        }
        else if (listPos == 1)
        {
            fly();
        }
    }

}
