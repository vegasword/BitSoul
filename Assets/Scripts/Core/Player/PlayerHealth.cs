using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private PlayerAnimationsEvents playerAnimationsEvents;

    [HideInInspector] public bool  isDead             = false;
    [HideInInspector] public bool  isParalyzed        = false;
    [HideInInspector] public float invincibleCooldown = 0f;
    [HideInInspector] public int   maxLives;

    [HideInInspector] public UnityEvent playerHealed;
    [HideInInspector] public UnityEvent playerDamaged;
    
    public int life = 3;

    public float invincibleTime = 2.5f;
    
    public Animator       transitionAnimator;
    public GameController gameController;
    public HealthBar      healthBar;

    void Start()
    {
        playerAnimationsEvents = GetComponent<PlayerAnimationsEvents>();
        
        if (playerDamaged == null) playerDamaged = new UnityEvent();
        if (playerHealed  == null) playerHealed  = new UnityEvent();

        // Events binding.
        playerDamaged.AddListener(PlayerDamagedEvent);
        playerHealed.AddListener(PlayerHealedEvent);

        maxLives = life;
    }

    void Update()
    {
        // Invincible timer update.
        if (invincibleCooldown > 0f)
            invincibleCooldown -= Time.deltaTime;
        else if (invincibleCooldown < 0.01f)
            invincibleCooldown  = 0f;

        // Set dead status.
        isDead = (life <= 0);

        // Player death animation.
        if (playerAnimationsEvents.deathAnimationDone)
        {
            transitionAnimator.SetTrigger("Start");
            if (transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Transition_Start") &&
                transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                gameController.playerIsDead.Invoke();
            }
        }
    }

    public void PlayerHealedEvent()
    {
        if (life < maxLives) life++;
        healthBar.RefresHealthBar();
    }

    private void PlayerDamagedEvent()
    {
        life--;
        healthBar.RefresHealthBar();

        isParalyzed        = true;
        invincibleCooldown = invincibleTime;
    }
}
