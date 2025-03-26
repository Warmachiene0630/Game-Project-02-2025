using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PlayerType : ScriptableObject
{

    [Header("----- Model -----")]
    public GameObject model;

    [Header("----- Stats -----")]
    [Range(1, 10)] public int HPMax;
    [Range(3, 10)]public float speedBase;
    [Range(2, 5)]public float sprintMod;
    [Range(5, 20)]public int jumpSpeed;
    [Range(1, 3)]public int jumpMax;

    [Header("----- Start Weapons -----")]
    public meleeStats assignedWeapon;
    public GunStats assignedGun;

    [Header("----- Bonuses -----")]
    public int meleeBonus;
    public int hpRecover;

    [Header("----- Audio -----")]
    public AudioClip[] audSteps;
    [Range(0, 1)]public float audStepsVol;
    public AudioClip[] audHurt;
    [Range(0, 1)]public float audHurtVol;
    public AudioClip[] audJump;
    [Range(0, 1)]public float audJumpVol;


    [Header("------ Remaining Stats ------")]
    public int livesLeft;
    public int totalGold;
    public int healthRemaining;
    public float remainigFuel;
    public bool firstScene;
    public List<GunStats> guns = new List<GunStats>();

}
