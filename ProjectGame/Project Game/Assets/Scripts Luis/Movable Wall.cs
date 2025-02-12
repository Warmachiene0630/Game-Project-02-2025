
using UnityEngine;
using UnityEngine.AI;

public class MovableWall : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    private float trapTimer;
    [SerializeField] int trapRate;
    [SerializeField] int trapDelay;
    [SerializeField] int trapSpeed;

    private Vector3 playerDistance;
    private bool trapMoved;
    [SerializeField] int trapTrigger;

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

        if((playerDistance.y <= trapTrigger || playerDistance.x <= trapTrigger || playerDistance.z <= trapTrigger))
        {
            moveTrap();

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
}
