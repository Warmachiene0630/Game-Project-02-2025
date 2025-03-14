using UnityEngine;

public class LavaAudioController : MonoBehaviour
{
    public AudioSource lavaAudio; // Assign the AudioSource
    public Transform player; // Assign the player in the inspector
    public float maxDistance = 15f; // Max distance for hearing the sound
    public float minVolume = 0.1f; // Minimum volume (far away)
    public float maxVolume = 0.5f; // Maximum volume (close)

    private void Update()
    {
        if (player == null || lavaAudio == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < maxDistance)
        {
            if (!lavaAudio.isPlaying)
                lavaAudio.Play();

            float volume = Mathf.Lerp(minVolume, maxVolume, 1 - (distance / maxDistance));
            lavaAudio.volume = volume;
        }
        else if (lavaAudio.isPlaying)
        {
            lavaAudio.Stop();
        }
    }
}