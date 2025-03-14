using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource footstepAudio;
    public AudioClip defaultFootstep; // Default footstep sound
    public AudioClip grassFootstep;   // Footsteps for grass
    public AudioClip iceFootstep;     // Footsteps for ice
    public AudioClip dirtFootstep;    // Footsteps for dirt

    private CharacterController controller;
    private float stepInterval = 0.5f;
    private float stepTimer = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (footstepAudio == null)
        {
            footstepAudio = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Reset timer when not moving
        }
    }

    private void PlayFootstep()
    {
        if (footstepAudio == null) return;

        AudioClip footstepSound = defaultFootstep;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            string surfaceTag = hit.collider.tag;

            switch (surfaceTag)
            {
                case "Grass":
                    footstepSound = grassFootstep;
                    break;
                case "Ice":
                    footstepSound = iceFootstep;
                    break;
                case "Dirt":
                    footstepSound = dirtFootstep;
                    break;
            }
        }

        footstepAudio.clip = footstepSound;
        footstepAudio.Play();
    }
}