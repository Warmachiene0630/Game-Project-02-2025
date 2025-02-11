using UnityEngine;
using UnityEngine.AI;

public class Traps : MonoBehaviour
{
    enum trapType {Moving, Stationary, Shooter}
    [SerializeField] trapType type;
    [SerializeField] Rigidbody rb;

    private float trapTimer;
    [SerializeField] int trapRate;
    [SerializeField] int trapDelay;
    [SerializeField] int trapSpeed;


    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    private Vector3 playerDistance;
    private bool trapMoved;
    [SerializeField] int trapTrigger;
    [SerializeField] int destroyTimer;

    [SerializeField] int damageAmount;
    [SerializeField] int destroyDelay;

    Vector3 trapStart;

    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = gamemanager.instance.player.transform.position - transform.position;
        
        if (type == trapType.Moving && ((playerDistance.y <= trapTrigger || playerDistance.x <= trapTrigger || playerDistance.z <= trapTrigger) || trapMoved == true))
        {
            moveTrap();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }

        if (type == trapType.Shooter)
        {
            Destroy(gameObject);
        }
    }

    private void moveTrap()
    {
        if (trapMoved == false && trapTimer >= trapRate)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(transform.position.x + 3, 0, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * trapSpeed);
            trapMoved = !trapMoved;
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(transform.position.x - 3, 0, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * trapSpeed);
            if (transform.position == trapStart)
            {
                trapMoved = !trapMoved;
            }
        }
    }

    void shoot()
    {
        trapTimer = 0;

        Instantiate(bullet, shootPos.position, transform.rotation);

    }
}
