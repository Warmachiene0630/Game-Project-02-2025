using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
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
    

    private float trapTimer;
    private bool shooting;
    [SerializeField] float trapRate;
    [SerializeField] float trapDelay;
    [SerializeField] float bulletsFire;

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

        Debug.DrawLine(shootPos.position, barrel.transform.forward * collider.radius, Color.blue);
        if (followPlayer == true && canSeePlayer() == true && shooting == true)
        {
            moveTurret();
        }

        if (followPlayer == false) {
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

    private bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - shootPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

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
        moveBarrel();
    }

    void moveBarrel()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(0, playerDir.y, 0));
        barrel.transform.rotation = Quaternion.Lerp(barrel.transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    void shoot()
    {
        trapTimer = 0;

        Instantiate(cannonBall, shootPos.position, transform.rotation);

    }
}
