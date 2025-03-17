using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip sfxClip;

    public float musicVol;
    public float sfxVol;
    public float masterVol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        musicSource.PlayOneShot(menuMusic, musicVol);
    }

    public void playSFXClip()
    {
        sfxSource.PlayOneShot(sfxClip, sfxVol);
    }
}
