using UnityEngine;

public enum PowerUpType { Speed, TeleportJump, Invisibility }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float boostAmount;
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && other.gameObject.activeInHierarchy)
        {
            PowerUpEffect effect = other.GetComponent<PowerUpEffect>();
            if (effect == null)
            {
                effect = other.gameObject.AddComponent<PowerUpEffect>();
            }
            effect.ApplyPowerUp(type, boostAmount, duration);
            Destroy(gameObject);
        }
    }
}