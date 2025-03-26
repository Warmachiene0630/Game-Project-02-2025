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
        Objective.instance.addDir(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            GameManager.instance.updateGameGoal(-1);
            Objective.instance.removeDir(transform.position);
            aud.PlayOneShot(keySound, keyVol);
            Destroy(gameObject);
        }
    }
}
