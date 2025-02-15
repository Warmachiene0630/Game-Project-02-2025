using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Traps : MonoBehaviour
{
    [SerializeField] Rigidbody rb;



    private float trapTimer;
    [SerializeField] float trapRate;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject cannonBall;
    [SerializeField] int trapTrigger;

    [SerializeField] int damageAmount;


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


        Destroy(gameObject);
    }
   

    
    void shoot()
    {
        trapTimer = 0;

        Instantiate(cannonBall, shootPos.position, transform.rotation);

    }
}
