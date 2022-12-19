using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionsEvents : MonoBehaviour
{
    private Rigidbody2D     playerRB;
    private BoxCollider2D   playerBC;
    private PlayerHealth    playerHealth;
    private PlayerMovements playerMovements;

    private BossMovements bossMovements;
    private BossHealth    bossHealth;
    private BossShoot     bossShoot;

    [SerializeField] private float heartDropRNG                = 0.35f;
    [SerializeField] private float bossBounciness              = 1.15f;
    [SerializeField] private float bossDamagedAcceleration     = 10f;
    [SerializeField] private float bossDamagedTrajectoryRadius = 4f;
    [SerializeField] private float bossDamagedFirerate         = 0.15f;

    public float bounciness = 12f;
    public float knockback  = 12f;

    public LayerMask ground;

    public GameObject healDrop;
    public GameObject boss;

    void Start()
    {
        playerRB        = GetComponent<Rigidbody2D>();
        playerBC        = GetComponent<BoxCollider2D>();
        playerHealth    = GetComponent<PlayerHealth>();
        playerMovements = GetComponent<PlayerMovements>();

        bossMovements = boss.gameObject.GetComponent<BossMovements>();
        bossHealth    = boss.gameObject.GetComponent<BossHealth>();
        bossShoot     = boss.gameObject.GetComponent<BossShoot>();
    }

    // ---------- TRIGGER ENTER ---------- //

    private void OnEnemyDamaged(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && other.isTrigger && playerRB.velocity.y < -1f)
        {
            // Make the player bounce.
            playerMovements.Bounce(bounciness);

            // Check if we collide the enemy head.
            if (other is CapsuleCollider2D && other.isTrigger)
            {
                // Spawn a Heal Drop if player hp are low.
                if (playerHealth.life < playerHealth.maxLives && Random.value <= heartDropRNG)
                {
                    GameObject drop = Instantiate(healDrop, other.bounds.center, Quaternion.identity);
                    drop.gameObject.GetComponent<HealDrop>().playerBC     = playerBC;
                    drop.gameObject.GetComponent<HealDrop>().playerHealth = playerHealth;
                }
            }

            Destroy(other.gameObject);
        }
    }

    private void OnBossDamaged(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boss") && bossHealth.invincibleCooldown == 0f && playerRB.velocity.y < -1f)
        {
            bossHealth.life--;
            bossHealth.invincibleCooldown = bossHealth.invincibleTime;

            bossMovements.movementsSpeed   += bossDamagedAcceleration;
            bossMovements.trajectoryRadius += bossDamagedTrajectoryRadius;

            bossShoot.fireRate -= bossDamagedFirerate;

            playerMovements.Bounce(bounciness * bossBounciness);
        }
    }

    private void OnPlayerDamaged(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            playerHealth.playerDamaged.Invoke();

            float direction = (playerBC.bounds.center.x - other.gameObject.GetComponent<Collider2D>().bounds.center.x) <= 0 ? -1f : 1f;
            playerMovements.Knockback(direction, knockback);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerHealth.life >= 0)
        {
            OnBossDamaged(other);
            OnEnemyDamaged(other);

            if (playerHealth.invincibleCooldown == 0f)
                OnPlayerDamaged(other);
        }
    }
    
    // ---------- COLLISION ENTER ---------- //

    private void OnPlayerDamaged(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            playerHealth.playerDamaged.Invoke();

            float direction = (playerBC.bounds.center.x - other.gameObject.GetComponent<Collider2D>().bounds.center.x) <= 0 ? -1f : 1f;
            playerMovements.Knockback(direction, knockback);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (playerHealth.life >= 0 && playerHealth.invincibleCooldown == 0f)
            OnPlayerDamaged(other);
    }
}
