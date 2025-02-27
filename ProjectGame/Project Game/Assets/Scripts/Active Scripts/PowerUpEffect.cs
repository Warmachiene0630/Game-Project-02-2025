using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    private CharacterController controller;
    private PlayerController player;
    private int originalLayer;

    private bool isSpeedBoosted;
    private float speedBoostAmount;
    private float speedBoostDuration;
    private float speedBoostTimer;

    private bool isTeleportJumpActive;
    private float teleportJumpHeight;
    private float teleportJumpDuration;
    private float teleportJumpTimer;
    private int jumpCount;
    private const int MAX_NORMAL_JUMPS = 2;
    private const int MAX_TOTAL_JUMPS = 3;
    private float lastJumpTime;
    private const float JUMP_TIMEOUT = 2f;

    private bool isInvisible;
    private float invisibilityDuration;
    private float invisibilityTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerController>();
        originalLayer = gameObject.layer;

        if (controller == null || player == null)
        {
            enabled = false;
            return;
        }
    }

    public void ApplyPowerUp(PowerUpType type, float boostAmount, float duration)
    {
        if (!enabled) return;
        switch (type)
        {
            case PowerUpType.Speed:
                isSpeedBoosted = true;
                speedBoostAmount = boostAmount;
                speedBoostDuration = duration;
                speedBoostTimer = 0;
                break;
            case PowerUpType.TeleportJump:
                isTeleportJumpActive = true;
                teleportJumpHeight = boostAmount;
                teleportJumpDuration = duration;
                teleportJumpTimer = 0;
                jumpCount = 0;
                lastJumpTime = -JUMP_TIMEOUT;
                break;
            case PowerUpType.Invisibility:
                if (!isInvisible)
                {
                    isInvisible = true;
                    invisibilityDuration = duration;
                    invisibilityTimer = 0;
                    gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                break;
        }
    }

    void Update()
    {
        if (!enabled) return;
        UpdateTimer(ref isSpeedBoosted, ref speedBoostTimer, speedBoostDuration, null);
        UpdateTimer(ref isTeleportJumpActive, ref teleportJumpTimer, teleportJumpDuration, null);
        UpdateTimer(ref isInvisible, ref invisibilityTimer, invisibilityDuration, () =>
        {
            gameObject.layer = originalLayer;
        });

        if (isSpeedBoosted && controller != null && controller.enabled)
        {
            Vector3 moveDir = (Input.GetAxis("Horizontal") * transform.right +
                               Input.GetAxis("Vertical") * transform.forward).normalized;
            controller.Move(moveDir * speedBoostAmount * Time.deltaTime);
        }

        if (isTeleportJumpActive && Input.GetButtonDown("Jump") && controller != null && controller.enabled)
        {
            float timeSinceLastJump = Time.time - lastJumpTime;
            if (controller.isGrounded)
            {
                jumpCount = 0;
                lastJumpTime = Time.time;
            }
            else if (!controller.isGrounded && timeSinceLastJump <= JUMP_TIMEOUT && jumpCount == MAX_NORMAL_JUMPS)
            {
                Vector3 targetPosition = transform.position + Vector3.up * teleportJumpHeight;
                StartCoroutine(SmoothTeleport(targetPosition, 0.1f));
                jumpCount = 0;
                lastJumpTime = Time.time;
            }
            else if (!controller.isGrounded)
            {
                jumpCount++;
                lastJumpTime = Time.time;
            }
            else
            {
                jumpCount = 0;
            }
        }
    }

    private System.Collections.IEnumerator SmoothTeleport(Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            controller.Move(newPosition - transform.position);
            yield return null;
        }
        controller.Move(targetPosition - transform.position);
    }

    private void UpdateTimer(ref bool isActive, ref float timer, float duration, System.Action onEnd)
    {
        if (!isActive) return;
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            isActive = false;
            onEnd?.Invoke();
        }
    }
}