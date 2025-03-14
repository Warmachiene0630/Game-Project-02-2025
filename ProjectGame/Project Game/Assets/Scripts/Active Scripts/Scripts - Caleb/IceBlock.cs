using UnityEngine;

public class IceBlock : MonoBehaviour
{
    private float slideFriction = 0.98f;
    private float slideSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetSliding(true, slideFriction, slideSpeed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetSliding(false, 1f, 0f);
            }
        }
    }
}