using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] int speedMod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.SendMessage("increaseSpeed", speedMod);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.SendMessage("decreaseSpeed", speedMod);
        }
    }
}
