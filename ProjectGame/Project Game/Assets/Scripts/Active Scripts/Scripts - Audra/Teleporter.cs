using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // if the teleport object is set as a trap, the player will instantly teleport
    //if it isnt a trap, the player will be given the option to press a button to teleport

    [SerializeField] Transform teleportPos;
    [SerializeField] AudioSource aud;

    [SerializeField] bool isTrap;
    public bool canTeleport;

    [Header("----- Audio -----")]
    public AudioClip teleSound;
    [Range(0, 1)] public float teleVol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (canTeleport && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Teleport());
            aud.PlayOneShot(teleSound, teleVol);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isTrap)
            {
                StartCoroutine(Teleport());
                aud.PlayOneShot(teleSound,teleVol);
            }
            else if (!isTrap)
            {
                canTeleport = true;
                GameManager.instance.teleportPopup.SetActive(true);
            }
        }
    }

    private void OnTriggerExit()
    {
        canTeleport = false;
        GameManager.instance.teleportPopup.SetActive(false);
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(.2f);
        GameManager.instance.playerScript.transform.position = teleportPos.transform.position;
        yield return new WaitForSeconds(.2f);
    }

}
