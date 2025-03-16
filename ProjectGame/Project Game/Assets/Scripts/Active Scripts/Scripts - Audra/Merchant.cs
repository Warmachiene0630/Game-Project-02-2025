using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    //players collect coins on enemy defeat and can use them to buy items from the merchant when in range
    
    bool canShop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canShop && Input.GetButtonDown("Interact"))
        {
            GameManager.instance.enterStore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canShop = true;
            GameManager.instance.merchantPopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canShop = false;
        GameManager.instance.merchantPopup.SetActive(false);
    }
}
