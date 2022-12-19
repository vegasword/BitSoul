using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    private float fireRateTimer;

    private Collider2D    bossCollider;
    private Animator      bossAnimator;
    private BossHealth    bossHealth;
    
    public float fireRate    = 1f;
    public float bulletSpeed = 3f;
    public float awakeRange  = 30f;

    public PlayerHealth  playerHealth;
    public BoxCollider2D playerBC;
    public GameObject    Bullet;

    void Start()
    {
        fireRateTimer = fireRate;
        bossCollider  = GetComponent<CapsuleCollider2D>();
        bossAnimator  = GetComponent<Animator>();
        bossHealth    = GetComponent<BossHealth>();
    }

    void Update()
    {
        fireRateTimer -= Time.deltaTime;

        if (fireRateTimer <= 0f                 && 
            bossHealth.invincibleCooldown == 0f &&
            !bossHealth.isDead                  &&
            Vector2.Distance(playerBC.bounds.center, bossCollider.bounds.center) <= awakeRange)
        {
            // Instanciate a bullet from the boss to the player.
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().InitBullet(playerBC, bulletSpeed);

            // Update values.
            fireRateTimer = fireRate;
            bossAnimator.Play("BossShoot", 0);
        }
    }
}
