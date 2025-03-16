using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] AudioSource aud;

    [Header("----- Audio -----")]
    public AudioClip keySound;
    [Range(0, 1)] public float keyVol;

    private void Start()
    {
        GameManager.instance.updateGameGoal(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.updateGameGoal(-1);
        aud.PlayOneShot(keySound, keyVol);
        Destroy(gameObject);
    }
}
