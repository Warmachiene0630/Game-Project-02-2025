using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TrackingBulltet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int slowTimer;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] int facePlayerSpeed;
    Vector3 playerDir;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        trackPlayer();
        rb.linearVelocity = transform.forward * speed;
     
    }

    void trackPlayer()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.slowSpeed(slowTimer);
        }

        Destroy(gameObject);
    }


}
