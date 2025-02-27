using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    [Header("----- Stats -----")]
    public GameObject model;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int ammoCur, ammoMax;

    public ParticleSystem hitEffect;

    [Header("----- Audio -----")]
    public AudioClip[] shootSound;
    [Range (0,1)] public float shootVol;
    public AudioClip[] reloadSound;
    [Range(0, 1)] public float reloadVol;
}
