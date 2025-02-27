using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LavaPit : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    private float trapTimer;
    private bool takingDmg;
    private IDamage dmg;

    [SerializeField] int damageAmount;

    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        takingDmg = false;
    }
    private void Update()
    {
        trapTimer += Time.deltaTime;
        if (takingDmg == true && (trapTimer >= 1))
        {
            dmg.takeDamage(damageAmount);
            trapTimer = 0;
        }

    }


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
            takingDmg = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        takingDmg = false;
    }
}
