using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;

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

    public void SetMusicVol()
    {
        float vol = musicSlider.value;
        mixer.SetFloat("musicVol", Mathf.Log10(vol)*20);
        PlayerPrefs.SetFloat("musicVolume", vol);
    }

    public void SetMasterVol()
    {
        float vol = masterSlider.value;
        mixer.SetFloat("masterVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("masterVolume", vol);
    }

    public void SetSFXVol()
    {
        float vol = sfxSlider.value;
        mixer.SetFloat("sfxVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", vol);
    }

    private void LoadVol()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVol();
        SetMasterVol();
        SetSFXVol();
    }
}
