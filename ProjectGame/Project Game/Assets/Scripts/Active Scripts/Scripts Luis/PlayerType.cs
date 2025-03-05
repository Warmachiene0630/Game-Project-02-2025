using UnityEngine;

[CreateAssetMenu]

public class PlayerType : ScriptableObject
{
    [Header("----- Model -----")]
    public GameObject model;

    [Header("----- Stats -----")]
    [Range(1, 10)] public int HP;
    [Range(3, 10)][SerializeField] float speed;
    [Range(2, 5)][SerializeField] float sprintMod;
    [Range(5, 20)][SerializeField] int jumpSpeed;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(15, 45)][SerializeField] int gravity;
    [Range(5, 15)][SerializeField] float speedBoostTime;
    [Range(5, 15)][SerializeField] float damageBoostTime;
    [Range(1, 5)][SerializeField] int damageBoostAmount;

    [Header("----- Bonuses -----")]
    public int meleeBonus;
    public int meleeSpeed;
    public int hpRecover;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
}
