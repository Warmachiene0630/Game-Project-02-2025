using UnityEngine;

public class FuelPack : MonoBehaviour
{
    [Header("----- Stats -----")]
    [SerializeField] int packStrength;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    public AudioClip fuelSound;
    [Range(0, 1)] public float fuelVol;

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
            IPickUp dmg = other.GetComponent<IPickUp>();
            if (dmg != null)
            {
                if (dmg.gainFuel(packStrength))
                {
                    aud.PlayOneShot(fuelSound, fuelVol);
                    Destroy(gameObject);

                }
            }
        }
    }
}
