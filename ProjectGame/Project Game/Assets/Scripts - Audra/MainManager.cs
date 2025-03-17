using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public float musicVol;
    public float sfxVol;
    public float masterVol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }
}
