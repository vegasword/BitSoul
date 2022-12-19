using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private SpriteRenderer    bossRenderer;
    private Rigidbody2D       bossRB;
    private CapsuleCollider2D bossCC;
    
    [HideInInspector] public float invincibleCooldown = 0f;
    
    public bool isDead;

    public int life = 5;

    public float invincibleTime          = 3f;
    public float invincibleBlinkingSpeed = 10f;

    public Animator       transitionAnimator;
    public GameController gameController;

    void Start()
    {
        bossRenderer = GetComponent<SpriteRenderer>();
        bossRB       = GetComponent<Rigidbody2D>();
        bossCC       = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        isDead = (life <= 0);

        if (!isDead)
        {
            // Invincible timer update.
            if      (invincibleCooldown > 0f)    invincibleCooldown -= Time.deltaTime;
            else if (invincibleCooldown < 0.01f) invincibleCooldown  = 0f;

            if (invincibleCooldown != 0f)
            {
                bossRenderer.color = Color.Lerp(new Color(bossRenderer.color.r, bossRenderer.color.g, bossRenderer.color.b, 0f),
                                                new Color(bossRenderer.color.r, bossRenderer.color.g, bossRenderer.color.b, 1f),
                                                Mathf.PingPong(Time.time * invincibleBlinkingSpeed, 1f));
                bossCC.enabled = false;
                                        
            }
            else
            {
                bossRenderer.color = Color.white;
                bossCC.enabled = true;
            }
        }
        else
        {
            transitionAnimator.SetTrigger("LevelEnd");

            bossRB.bodyType       = RigidbodyType2D.Dynamic;
            bossRB.freezeRotation = false;
            bossCC.enabled        = false;

            transform.Rotate(new Vector3(0f, 0f, 200f * Time.deltaTime), Space.Self);

            if (transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("EndScreen_Transition") &&
                transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                gameController.bossIsDead.Invoke();
            }
        }
    }
}
