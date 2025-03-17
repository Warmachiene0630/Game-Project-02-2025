using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip sfxClip;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVol();
        }
        else {
            SetMusicVol();
            SetMasterVol();
            SetSFXVol();
        }
    }

    private void Update()
    {
        float sfxOrigVol = MainManager.instance.sfxVol;
        MainManager.instance.musicVol = musicSlider.value;
        //GameManager.instance.musicVol = MainManager.instance.musicVol;
        mixer.SetFloat("musicVol", Mathf.Log10(MainManager.instance.musicVol) * 20);
        PlayerPrefs.SetFloat("musicVolume", MainManager.instance.musicVol);
        MainManager.instance.masterVol = masterSlider.value;
        mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.masterVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.masterVol);
        MainManager.instance.sfxVol = sfxSlider.value;
        mixer.SetFloat("sfxVol", Mathf.Log10(MainManager.instance.sfxVol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", MainManager.instance.sfxVol);
        if (sfxOrigVol != MainManager.instance.sfxVol)
        {
            sfxSource.PlayOneShot(sfxClip, MainManager.instance.sfxVol);
        }
        
    }

    public void SetMusicVol()
    {
        float vol = musicSlider.value;
        MainManager.instance.musicVol = vol;
        mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.musicVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.musicVol);
    }

    public void SetMasterVol()
    {
        float vol = masterSlider.value;
        MainManager.instance.masterVol = vol;
        mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.masterVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.masterVol);
    }

    public void SetSFXVol()
    {
        float vol = sfxSlider.value;
        MainManager.instance.sfxVol = vol;
        mixer.SetFloat("sfxVol", Mathf.Log10(MainManager.instance.sfxVol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", MainManager.instance.sfxVol);
    }

    private void LoadVol()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        MainManager.instance.musicVol = PlayerPrefs.GetFloat("musicVolume");
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        MainManager.instance.masterVol = PlayerPrefs.GetFloat("masterVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        MainManager.instance.sfxVol = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVol();
        SetMasterVol();
        SetSFXVol();
    }
}
