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
<<<<<<< HEAD:ProjectGame/Project Game/Assets/Scripts Luis/CheckPoint.cs
        if (other.CompareTag("Player"))
=======
        if (other.CompareTag("Player") && GameManager.instance.playerSpawnPos.transform.position != transform.position)
>>>>>>> main:ProjectGame/Project Game/Assets/Scripts/Active Scripts/Scripts Luis/CheckPoint.cs
        {
            GameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(flashColor());
        }
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        GameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
        GameManager.instance.checkpointPopup.SetActive(false);
    }
}
<<<<<<< HEAD:ProjectGame/Project Game/Assets/Scripts Luis/CheckPoint.cs
=======

>>>>>>> main:ProjectGame/Project Game/Assets/Scripts/Active Scripts/Scripts Luis/CheckPoint.cs
