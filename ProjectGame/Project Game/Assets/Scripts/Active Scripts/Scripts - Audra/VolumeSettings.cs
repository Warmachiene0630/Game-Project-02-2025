using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    //[SerializeField] private AudioMixer mixer;
    //private Slider musicSlider = MainManager.instance.musicSlider;
    //private Slider sfxSlider = MainManager.instance.sfxSlider;
    //private Slider masterSlider = MainManager.instance.masterSlider;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip sfxClip;
    int sceneOrig = 0;

    private void Start()
    {
        //musicSlider.value = MainManager.instance.musicVol;
        //masterSlider.value = MainManager.instance.masterVol;
        //sfxSlider.value = MainManager.instance.sfxVol;
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
        if(SceneManager.GetActiveScene().buildIndex == 0)
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
                sfxSource.PlayOneShot(sfxClip, MainManager.instance.sfxVol);
            }
        }
        else if(SceneManager.GetActiveScene().buildIndex != sceneOrig)
        {
            Start();
            sceneOrig++;
        }
        else
        {
            float sfxOrigVol = GameManager.instance.sfxVol;
            GameManager.instance.musicVol = MainManager.instance.musicSlider.value;
            MainManager.instance.mixer.SetFloat("musicVol", Mathf.Log10(GameManager.instance.musicVol) * 20);
            PlayerPrefs.SetFloat("musicVolume", GameManager.instance.musicVol);
            GameManager.instance.masterVol = MainManager.instance.masterSlider.value;
            MainManager.instance.mixer.SetFloat("masterVol", Mathf.Log10(GameManager.instance.masterVol) * 20);
            PlayerPrefs.SetFloat("masterVolume", GameManager.instance.masterVol);
            GameManager.instance.sfxVol = MainManager.instance.sfxSlider.value;
            MainManager.instance.mixer.SetFloat("sfxVol", Mathf.Log10(GameManager.instance.sfxVol) * 20);
            PlayerPrefs.SetFloat("sfxVolume", GameManager.instance.sfxVol);
            if (sfxOrigVol != GameManager.instance.sfxVol)
            {
                sfxSource.PlayOneShot(sfxClip, GameManager.instance.sfxVol);
            }
        }
        
    }


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
