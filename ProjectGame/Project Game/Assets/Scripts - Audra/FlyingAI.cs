using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class FlyingAI : MonoBehaviour, IDamage
{

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform headPos;
    [SerializeField] int HP;
    //[SerializeField] int animTransSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;


    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    //[SerializeField] float angleToShootAtPlayer = 0.1f;

    Color colorOrig;

    float shootTimer;
    float angleToPlayer;

    Vector3 playerDir;

    bool playerInRange;

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.updateGameGoal(-1);
        }
    }

    private void FacePlayer(Vector3 targetPOS)
    {
        Vector3 dir = targetPOS - transform.position;
        if(dir.sqrMagnitude < 0.0001f)
        {
            return;
        }
        dir.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * faceTargetSpeed);
    }

    private bool isFacingPlayer(float angle)
    {
        if (!playerInRange)
        {
            return true;
        }
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        return angleToPlayer <= angle;

    }

    private IEnumerator RotateUntilFacingPlayer(float angle)
    {
        while (!isFacingPlayer(angle))
        {
            FacePlayer(playerDir);
            yield return null;
        }
    }

    

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
