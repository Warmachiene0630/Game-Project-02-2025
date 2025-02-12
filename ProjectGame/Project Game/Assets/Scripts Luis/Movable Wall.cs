
using UnityEngine;
using UnityEngine.AI;

public class MovableWall : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    private float trapTimer;

    //timer count
    [SerializeField] float trapRate;
    //speed wall moves
    [SerializeField] int trapSpeed;

    private Vector3 playerDistance;
    private bool trapMoved;
    [SerializeField] int trapTrigger;

    Vector3 trapVel;

    Vector3 trapStart;

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

        moveTrap();

        

    }

    private void moveTrap()
    {
        if (trapTimer <= trapRate)
        {
            trapVel.x = trapSpeed;
        }
        else
        {
            trapVel.x = -trapSpeed;
            if (trapTimer == 2 * trapRate)
            {
                trapTimer = 0;
            }
        }
    }
}
