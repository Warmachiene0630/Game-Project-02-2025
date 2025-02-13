using UnityEngine;
using UnityEngine.AI;

public class Traps : MonoBehaviour
{
    enum trapType {Stationary, Shooter}
    [SerializeField] trapType type;
    [SerializeField] Rigidbody rb;

    private float trapTimer;
    [SerializeField] float trapRate;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject cannonBall;

    private Vector3 playerDistance;
    [SerializeField] int trapTrigger;

    [SerializeField] int damageAmount;

    Vector3 trapStart;

    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        trapTimer += Time.deltaTime;
        if (trapTimer >= trapRate)
        {
            shoot();
        }
    }
  

    // Update is called once per frame
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


    void shoot()
    {
        trapTimer = 0;

        Instantiate(cannonBall, shootPos.position, transform.rotation);

    }
}
