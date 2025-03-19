using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class checkpoint : MonoBehaviour
{
    [SerializeField] Renderer model;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player") && GameManager.instance.playerSpawnPos.transform.position != transform.position)
            {
                GameManager.instance.playerSpawnPos.transform.position = model.transform.position;
                StartCoroutine(flashColor());
            } 
    }

    IEnumerator flashColor()
    {
        GameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.checkpointPopup.SetActive(false);
    }
}
 
