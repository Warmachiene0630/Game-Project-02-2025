using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovableWall : MonoBehaviour
{
    enum wallType {forward, up}

    [SerializeField] Rigidbody rb;
    [SerializeField] wallType type;

    private float trapTimer;
    [SerializeField] float trapDelay;

    //timer count
    [SerializeField] float trapRate;
    private bool playerInRange; 

    //speed wall moves
    [SerializeField] int trapSpeed;
    private bool trapMoved;
    private bool active;
    [SerializeField] bool moveTrapOnce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trapMoved = false;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInRange || active == true) {
            trapTimer += Time.deltaTime;
            active = true;
            if (type == wallType.forward)
            {

                if (trapMoved == false)
                {
                    moveTrapFor();
                }
                else
                {
                    if (trapTimer >= trapDelay)
                    {
                        trapMoved = false;
                        trapTimer = 0;
                    }
                }

            } else if (type == wallType.up)
            {
                if (trapMoved == false)
                {
                    moveTrapUp();
                }
                else
                {
                    if (trapTimer >= trapDelay)
                    {
                        trapMoved = false;
                        trapTimer = 0;
                    }
                }
            }
        }
    }

    private void moveTrapFor()
    {
       if (trapTimer <= trapRate)
       {
           rb.linearVelocity = transform.forward * trapSpeed;
       }
       else
       {
            if (moveTrapOnce == false)
            {

           
                rb.linearVelocity = transform.forward * -trapSpeed;
                if (trapTimer >= (2 * trapRate))
                {
                    trapTimer = 0;


                }
            }
            else
            {
                rb.linearVelocity = transform.forward * 0;
            }
       }
    }

    public void moveTrapUp()
    {
       
        if (trapTimer <= trapRate)
        {
           rb.linearVelocity = transform.up * trapSpeed;
        }
        else
        {
           if (trapTimer <= (3 * trapRate))
           {
              rb.linearVelocity = transform.up * 0;
           }
           else if(trapTimer <= (4 * trapRate))
           {
                rb.linearVelocity = -transform.up * trapSpeed;

           }
           else if(trapTimer <= (5 * trapRate))
           {
                trapTimer = 0;
                active = false;
                trapMoved = true;
            }
       }
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

}
