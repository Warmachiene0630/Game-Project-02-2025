using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Traps : MonoBehaviour
{
    public bool followPlayer;
    [SerializeField] SphereCollider collider;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject barrel;


    [SerializeField] int facePlayerSpeed;
    [SerializeField] int FOV;
    Vector3 playerDir;
    private float angleToPlayer;
    public bool slowTrap;
    

    private float trapTimer;
    private bool shooting;
    private bool trapShot;
    [SerializeField] float trapRate;
    [SerializeField] float trapDelay;
    [SerializeField] float bulletsFire;
    private float bulletsShot;


    [SerializeField] Transform shootPos;
    [SerializeField] GameObject cannonBall;


    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        shooting = false;
    }

    private void Update()
    {

        trapTimer += Time.deltaTime;



        Debug.DrawLine(shootPos.position, transform.forward * 10, Color.blue);
        if (followPlayer == true && canSeePlayer() == true && shooting == true)
        {
            moveTurret();
            moveBarrel();
        }
        angleToPlayer = Vector3.Angle(playerDir, shootPos.transform.forward);
        if (trapShot == false)
        {
            if (followPlayer == false)
            {
                if (trapTimer >= trapRate && shooting == true)
                {
                    shoot();
                }
            }
            else
            {
                if (trapTimer >= trapRate && shooting == true)
                {
                    if (angleToPlayer <= FOV)
                    {
                        shoot();
                    }
                }
            }
        }
        else
        {
            if (trapTimer >= trapDelay)
            {
                trapShot = false;
                bulletsShot = 0;
            }
        }
    }

    private bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - shootPos.position;

        RaycastHit hit;
        if (Physics.Raycast(shootPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
  

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            shooting = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shooting = false;
        }
    }

    void moveTurret()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    
    }

    void moveBarrel()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        barrel.transform.rotation = Quaternion.Lerp(barrel.transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    void shoot()
    {
        trapTimer = 0;

        Instantiate(cannonBall, shootPos.position, shootPos.transform.rotation);
        bulletsShot += 1;
        if (bulletsShot >= bulletsFire)
        {
            trapShot = true;
        }

    }
}
