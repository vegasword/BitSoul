using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool destroyed;

    private Rigidbody2D      bulletRB;
    private CircleCollider2D bulletCC;
    
    public float Lifespan = 3f;

    public void InitBullet(BoxCollider2D playerBC, float speed)
    {
        bulletCC = GetComponent<CircleCollider2D>();
        bulletRB = GetComponent<Rigidbody2D>();
        bulletRB.velocity = (playerBC.bounds.center - bulletCC.bounds.center).normalized * speed;
    }

    void Update()
    {
        Lifespan -= Time.deltaTime;

        if (Lifespan <= 0f) Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground")) Destroy(this.gameObject);
    }
}
