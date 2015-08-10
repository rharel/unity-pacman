using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Handles interactions involving the player:
///     - Coin pickups.
///     - Ghost kills.
///     - Powerups
///     - Death
/// </summary>
public class PlayerInteraction : MonoBehaviour 
{
    /// <summary>
    /// Number of seconds the player is immune to ghosts after respawning.
    /// </summary>
    public float immunityTimeout = 1;
    /// <summary>
    /// Number of seconds before the effects of a powerup wear-off.
    /// </summary>
    public float powerupTimeout = 5;

    public event Action CoinPickup = delegate { };
    public event Action GhostKill = delegate { };
    public event Action PowerupOn = delegate { };
    public event Action PowerupOff = delegate { };
    public event Action Death = delegate { };

    private static readonly string ANIMATOR_IMMUNITY = "Immune";
    private static readonly string ANIMATOR_POWERUP_REMAINING = "Powerup Remaining";

    private float immunityStartTime;
    private bool isImmune = false;
    public bool IsImmune
    {
        get { return isImmune; }
        private set
        {
            isImmune = value;
            animator.SetBool(ANIMATOR_IMMUNITY, value);

            if (isImmune)
                immunityStartTime = Time.fixedTime;
        }
    }

    private float powerupRemaining = 0;
    public bool IsPoweredUp
    {
        get { return powerupRemaining > 0; }
        private set
        {
            powerupRemaining = value ? 1 : 0;
            animator.SetFloat(ANIMATOR_POWERUP_REMAINING, powerupRemaining);
        }
    }

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (IsImmune && TimeUtility.IsTimeout(immunityStartTime, Time.fixedTime, immunityTimeout))
            IsImmune = false;

        if (IsPoweredUp)
        {
            powerupRemaining -= Time.fixedDeltaTime / powerupTimeout;
            animator.SetFloat(ANIMATOR_POWERUP_REMAINING, powerupRemaining);
            if (!IsPoweredUp)
                PowerupOff();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject other)
    {
        switch (other.tag)
        {
            case "Ghost": HandleGhostCollision(other.gameObject); break;
            case "Coin": HandleCoinCollision(other.gameObject); break;
            case "Powerup": HandlePowerupCollision(other.gameObject); break;
            default: return;
        }
    }

    private void HandleGhostCollision(GameObject ghost)
    {
        if (IsPoweredUp)
        {
            var movementController = ghost.GetComponent<MovementController>();
            movementController.Respawn();
            
            GhostKill();
        }
        else if (!IsImmune)
            Die();
    }

    private void HandleCoinCollision(GameObject coin)
    {
        Destroy(coin);
        CoinPickup();
    }

    private void HandlePowerupCollision(GameObject powerup)
    {
        Destroy(powerup);
        IsPoweredUp = true;
        PowerupOn();
    }

    private void Die()
    {
        DebugUtility.Assert(!IsImmune);
        DebugUtility.Assert(!IsPoweredUp);
        
        IsImmune = true;

        var movementController = GetComponent<MovementController>();
        movementController.Respawn();

        Death();
    }
}
