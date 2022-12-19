using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaController : MonoBehaviour
{
    private bool wallHit      = false;
    private bool detectBorder = false;
    private bool detectPlayer = false;

    private float speed;

    private Rigidbody2D      koopaRB;
    private CircleCollider2D koopaCC;
    private Animator         koopaAnimator;

    [Range(-1f, 1f)] public int direction = -1;

    public float normalSpeed    = 100f;
    public float agressiveSpeed = 150f;
    public float detectionRange = 2f;

    public LayerMask ground;

    public BoxCollider2D   playerBC;
    public PlayerMovements playerMovements;
 
    void Start()
    {
        speed                = normalSpeed;
        koopaAnimator        = GetComponent<Animator>();
        koopaRB              = GetComponent<Rigidbody2D>();
        koopaCC              = GetComponent<CircleCollider2D>();
        transform.localScale = new Vector2(direction, 1f);

        // If the direction is null, this one is random.
        if (direction == 0) direction = (int)(Random.value >= 0.5f ? -1 : 1);
    }

    void Update()
    {
        // Wall hit detection.
        wallHit = Physics2D.CircleCast(koopaCC.bounds.center, koopaCC.radius, new Vector2(direction, 0), 0.1f, ground).collider != null;

        // Detect border.
        detectBorder = !Physics2D.CircleCast(koopaCC.bounds.center, koopaCC.radius, new Vector2(direction * 25f, -1f), 1f, ground).collider;

        // Player detection.
        detectPlayer = Vector2.Distance(playerBC.bounds.center, koopaCC.bounds.center) <= detectionRange && playerMovements.grounded;

        // Update Koopa movements.
        if ((wallHit || detectBorder) && detectPlayer == false)
        {
            direction *= -1;
            speed = normalSpeed;
            koopaAnimator.Play("KoopaTurn");
        }

        // Freeze Koopa during the turn animation then reset to walking animation.
        if (koopaAnimator.GetCurrentAnimatorStateInfo(0).IsName("KoopaTurn"))
        {
            if (koopaAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                speed = normalSpeed;
                koopaAnimator.SetTrigger("Reset");
            }
            else
            {
                speed = 0f;
                detectPlayer = false;
            }
        }

        // Koopa run to player when detecting him.
        if (detectPlayer)
        {
            direction = Mathf.Clamp01(playerBC.bounds.center.x - transform.position.x) == 0 ? -1 : 1;
            if (direction == 0) direction = -1;
            speed = agressiveSpeed;
        }

        // Update Koopa facing.
        transform.localScale = new Vector2((float)direction, 1f);

        koopaRB.velocity = new Vector2((float)direction * speed * Time.fixedDeltaTime, koopaRB.velocity.y);
        koopaAnimator.SetFloat("Speed", speed / normalSpeed);
    }
}
