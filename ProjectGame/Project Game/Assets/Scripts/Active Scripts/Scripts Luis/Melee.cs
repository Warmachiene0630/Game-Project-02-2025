using UnityEngine;

public class Melee : MonoBehaviour
{

    public int damageAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.CompareTag("Enemy"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);
            }
        }
        return;
    }
}