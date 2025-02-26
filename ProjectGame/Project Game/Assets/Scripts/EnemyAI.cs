using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [Range(1, 15)] [SerializeField] int HP;
    [Range(1, 10)] [SerializeField] int animTransSpeed;
    [Range(1, 15)] [SerializeField] int faceTargetSpeed;
    [Range(45, 180)] [SerializeField] int FOV;

    [Header("----- Weaponry -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [Range(1, 5)] [SerializeField] float shootRate;
    [SerializeField] bool trackingBullets;

    Color colorOrig;

    float shootTimer;
    float angleToPlayer;

    Vector3 playerDir;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animCurSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.MoveTowards(animCurSpeed, agentSpeed, Time.deltaTime * animTransSpeed));

        shootTimer += Time.deltaTime;

        if (playerInRange && canSeePlayer())
        {

        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (shootTimer >= shootRate)
                {
                    shoot();
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }

        }
        return false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.updateGameGoal(-1);
            //coin drop on defeat
            GameManager.instance.updateCoinCount(Random.Range(0, 50));
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void shoot()
    {
        shootTimer = 0;
        if (trackingBullets == false)
        {
            Instantiate(bullet, shootPos.position, transform.rotation);
        }
        else
        {
            Instantiate(bullet, shootPos.position, GameManager.instance.player.transform.rotation);
        }
    }

    public bool gainHealth(int amount)
    {
        HP += amount;
        return true;
    }
}