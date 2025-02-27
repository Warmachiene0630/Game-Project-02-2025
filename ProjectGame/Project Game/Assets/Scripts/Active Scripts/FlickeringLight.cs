using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light flickerLight;
    public float minIntensity = 0.5f; // Minimum brightness
    public float maxIntensity = 2f;  // Maximum brightness
    public float flickerSpeed = 0.1f; // Speed of flickering

    void Start()
    {
        flickerLight = GetComponent<Light>();
    }

    void Update()
    {
        if (flickerLight != null)
        {
            // Randomly change the light intensity
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}