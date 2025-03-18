using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
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

    /*private void Update()
    {
         float sfxOrigVol = MainManager.instance.sfxVol;
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
    }*/


    public void SetMusicVol()
    {
        float vol = MainManager.instance.musicSlider.value;
        MainManager.instance.musicVol = vol;
        MainManager.instance.mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.musicVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.musicVol);
    }

    public void SetMasterVol()
    {
        float vol = MainManager.instance.masterSlider.value;
        MainManager.instance.masterVol = vol;
        MainManager.instance.mixer.SetFloat("masterVol", Mathf.Log10(MainManager.instance.masterVol) * 20);
        PlayerPrefs.SetFloat("masterVolume", MainManager.instance.masterVol);
    }

    public void SetSFXVol()
    {
        float vol = MainManager.instance.sfxSlider.value;
        MainManager.instance.sfxVol = vol;
        MainManager.instance.mixer.SetFloat("sfxVol", Mathf.Log10(MainManager.instance.sfxVol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", MainManager.instance.sfxVol);
    }

    public void LoadVol()
    {
        MainManager.instance.musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        MainManager.instance.musicVol = PlayerPrefs.GetFloat("musicVolume");
        MainManager.instance.masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        MainManager.instance.masterVol = PlayerPrefs.GetFloat("masterVolume");
        MainManager.instance.sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        MainManager.instance.sfxVol = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVol();
        SetMasterVol();
        SetSFXVol();
    }
}
