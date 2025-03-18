using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;
    [SerializeField] public Slider masterSlider;
    [SerializeField] public AudioMixer mixer;

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip sfxClip;

    public float musicVol;
    public float sfxVol;
    public float masterVol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        if(menuMusic != null)
        {
            musicSource.PlayOneShot(menuMusic, musicVol);
        }
    }

    private void Update()
    {
        float sfxOrigVol = sfxVol;
        MainManager.instance.musicVol = MainManager.instance.musicSlider.value;
        MainManager.instance.mixer.SetFloat("musicVol", Mathf.Log10(MainManager.instance.musicVol) * 20);
        PlayerPrefs.SetFloat("musicVolume", MainManager.instance.musicVol);
        MainManager.instance.masterVol = MainManager.instance.masterSlider.value;
        MainManager.instance.mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.masterVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.masterVol);
        MainManager.instance.sfxVol = MainManager.instance.sfxSlider.value;
        MainManager.instance.mixer.SetFloat("sfxVol", Mathf.Log10(MainManager.instance.sfxVol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", MainManager.instance.sfxVol);
        if (sfxOrigVol != MainManager.instance.sfxVol)
        {
            MainManager.instance.playSFXClip();
        }
    }

    public void playSFXClip()
    {
        sfxSource.PlayOneShot(sfxClip, sfxVol);
    }
}
