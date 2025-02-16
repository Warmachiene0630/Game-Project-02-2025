using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int packStrength;
    
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
                if (dmg.gainHealth(5))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
