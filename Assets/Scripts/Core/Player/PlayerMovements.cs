using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D   playerRB;
    private BoxCollider2D playerBC;
    private PlayerHealth  playerHealth;

    [HideInInspector] public bool  isBumping;
    [HideInInspector] public bool  facing;
    [HideInInspector] public bool  grounded;
    [HideInInspector] public float direction;

    public float movementSpeed       = 300f;
    public float jumpForce           = 8f;
    public float fallMultiplier      = 4f;
    public float lowJumpMultiplier   = 6f;
    public float knockbackFreezeTime = 1.5f;
    public float groundCheckRadius   = 0.1f;

    public LayerMask ground;

    public

    void Start()
    { 
        playerRB     = GetComponent<Rigidbody2D>();
        playerBC     = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        if (!playerHealth.isDead)
        {
            // Check if player is grounded.
            grounded = Physics2D.BoxCast(playerBC.bounds.center, playerBC.bounds.size, 0f, Vector2.down, 0.1f, ground);


            // Player move if he is not paralyzed.
            if (!playerHealth.isParalyzed)
            {
                // Set X-axis direction according to the horizontal input.
                direction = Input.GetAxisRaw("Horizontal");

                // Set the horizontal velocity of the player rigid body.
                playerRB.velocity = new Vector2(direction * movementSpeed * Time.deltaTime, playerRB.velocity.y);
            }
            else if (grounded && playerHealth.invincibleCooldown < playerHealth.invincibleTime / 1.25f)
            {
                playerHealth.isParalyzed = false;
            }

            // Player flip update.
            if ((!facing && direction > 0f ) || (facing && direction < 0f))
            {
                facing = !facing;
                transform.localScale *= new Vector2(-1f, 1f);
            }
        }
    }

    void Update()
    {
        if (!playerHealth.isDead)
        {
            // Jump update.
            if (Input.GetButtonDown("Jump") && grounded)
            {
                playerRB.velocity = Vector2.up * jumpForce;
                isBumping = false;
            }

            // Air controll (fall and low jump multiplier).
            if (!grounded)
            {
                if (playerRB.velocity.y < 0f)
                    playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier    - 1f) * Time.deltaTime;
                else if (playerRB.velocity.y > 0f && !Input.GetButton("Jump") && !isBumping)
                    playerRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
                else if (isBumping)
                    playerRB.velocity += Vector2.up * Physics2D.gravity.y * 2f * Time.deltaTime;
            }
        }
        else if (playerBC.enabled)
        {
            playerRB.velocity = Vector2.zero;
            playerRB.bodyType = RigidbodyType2D.Static;
            playerBC.enabled  = false;
        }
    }

    public void Bounce(float power)
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, power);
        isBumping = true;
    }

    public void Knockback(float direction, float power)
    {
        playerRB.velocity = new Vector2(power * direction, power);
    }
}