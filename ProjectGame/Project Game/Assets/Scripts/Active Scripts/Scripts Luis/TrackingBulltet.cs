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
        playerDir = new Vector3(GameManager.instance.player.transform.position.x - transform.position.x , GameManager.instance.player.transform.position.y - transform.position.y + 1, GameManager.instance.player.transform.position.z - transform.position.z);
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
        playerDir = new Vector3(GameManager.instance.player.transform.position.x - transform.position.x, GameManager.instance.player.transform.position.y - transform.position.y + 1, GameManager.instance.player.transform.position.z - transform.position.z);
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
