using UnityEngine;

public class PickupGuns : MonoBehaviour
{
    [SerializeField] GunStats gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gun.ammoCurr = gun.ammoMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickUp pick = other.GetComponent<IPickUp>();
        if (pick != null)
        {
            pick.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
