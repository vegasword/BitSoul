using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDrop : MonoBehaviour
{
    private bool  hasHealed;
    private float lifetime;

    private Rigidbody2D      heartdropRB;
    private CircleCollider2D heartdropCC;

    public bool  hasLifespan;
    public float lifespan;
    public float catchRange;
    public float catchSpeed;

    public BoxCollider2D playerBC     { get; set; }
    public PlayerHealth  playerHealth { get; set; }

    void Awake()
    {
        heartdropRB = GetComponent<Rigidbody2D>();
        heartdropCC = GetComponent<CircleCollider2D>();

        float power = Random.Range(0.4f, 0.7f);
        heartdropRB.AddRelativeForce(new Vector2(Random.Range(-1f, 1f), power), ForceMode2D.Impulse);
    }

    void Update()
    {
        // Lifespan update.
        if (hasLifespan)
        {
            if (lifetime <= lifespan)
                lifetime += Time.deltaTime;
            else 
                Destroy(this.gameObject);
        }
        
        // Item movements to player when range is crossed.
        if (Vector2.Distance(playerBC.bounds.center, heartdropCC.bounds.center) <= catchRange && playerHealth.life != playerHealth.maxLives)
            heartdropRB.velocity = (playerBC.bounds.center - heartdropCC.bounds.center) * catchSpeed * Time.deltaTime;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasHealed)
        {
            playerHealth.playerHealed.Invoke();
            hasHealed = true;
            Destroy(this.gameObject);
        }
    }
}