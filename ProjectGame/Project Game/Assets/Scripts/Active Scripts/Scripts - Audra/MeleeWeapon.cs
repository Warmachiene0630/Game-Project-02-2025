using UnityEngine;

[CreateAssetMenu]

public class meleeStats : ScriptableObject
{
    [Header("----- Stats -----")]
    public GameObject model;
    public int meleeDamage;
    public float meleeSpeed;

    public bool twoHanded;

    public ParticleSystem hitEffect;

    [Header("----- Audio -----")]
    public AudioClip[] hitSound;
    [Range(0, 1)] public float hitVol;
    public AudioClip[] swingSound;
    [Range(0, 1)] public float swingVol;
}
