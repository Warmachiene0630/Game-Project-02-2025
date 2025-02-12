
using UnityEngine;
using UnityEngine.AI;

public class MovableWall : MonoBehaviour
{
    enum wallType {forward, up }

    [SerializeField] Rigidbody rb;
    [SerializeField] wallType type;

    [SerializeField] float trapTimer;
    [SerializeField] float trapDelay;

    //timer count
    [SerializeField] float trapRate;
    //speed wall moves
    [SerializeField] int trapSpeed;

    private Vector3 playerDistance;
    bool trapMoved; 
    [SerializeField] int trapTrigger;

    Vector3 trapVel;

    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = transform.position;

        trapTimer += Time.deltaTime;
        if (type == wallType.forward)
        {
            moveTrapFor();
        }else if (type == wallType.up)
        {
            if (trapMoved == false) {
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

    private void moveTrapFor()
    {
       if (trapTimer <= trapRate)
       {
           rb.linearVelocity = transform.forward * trapSpeed;
       }
       else
       {
           rb.linearVelocity = transform.forward * -trapSpeed;
           if (trapTimer >= (2 * trapRate))
           {
               trapTimer = 0;
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
                trapMoved = true;                    
           }
       }
    }

}
