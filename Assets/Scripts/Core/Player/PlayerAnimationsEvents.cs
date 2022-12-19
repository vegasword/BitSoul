using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsEvents : MonoBehaviour
{
    private SpriteRenderer  playerRenderer;
    private Animator        playerAnimator;
    private PlayerMovements playerMovements;
    private Rigidbody2D     playerRB;
    private PlayerHealth    playerHealth;

    [HideInInspector]
    public bool deathAnimationDone = false;

    public float InvincibleBlinkingSpeed = 10f;

    void Start()
    {
        playerRenderer  = GetComponent<SpriteRenderer>();
        playerAnimator  = GetComponent<Animator>();
        playerMovements = GetComponent<PlayerMovements>();
        playerRB        = GetComponent<Rigidbody2D>();
        playerHealth    = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Update player animator value.
        playerAnimator.SetFloat("Speed"  , playerRB.velocity.magnitude);
        playerAnimator.SetFloat("Jumping", playerMovements.grounded ? 1f : 0f);
        playerAnimator.SetBool( "IsDead" , playerHealth.isDead);

        deathAnimationDone = (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") && 
                              playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        PlayerBlinking();
    }

    /// <summary>
    /// Play blinking animation when player getting hurted.
    /// </summary>
    public void PlayerBlinking()
    {
        if (!playerHealth.isDead)
        {
            if (playerHealth.invincibleCooldown > 0f)
            {
                Color alpha0, alpha1;
                alpha0 = alpha1 = Color.white;
                alpha0.a = 0f;
                playerRenderer.color = Color.Lerp(alpha0, alpha1, Mathf.PingPong(Time.time * InvincibleBlinkingSpeed, 1f));
            }
            else
            {
                playerRenderer.color = Color.white;
            }
        }
        else
        {
            playerRenderer.color = Color.white;
        }
    }
}
