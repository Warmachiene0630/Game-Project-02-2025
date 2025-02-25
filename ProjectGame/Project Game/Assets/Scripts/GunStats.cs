using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public GameObject model;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int ammoCurr, ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    public float shootVol;
}
