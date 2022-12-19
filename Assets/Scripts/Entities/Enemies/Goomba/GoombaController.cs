using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour
{
    private bool wallHit = false;

    private Rigidbody2D   goombaRB;
    private BoxCollider2D goombaBC;
    private Animator      goombaAnimator;

    [Range(-1f, 1f)] public int direction;

    public float speed = 100f;

    public LayerMask ground;
 
    void Start()
    {
        goombaRB       = GetComponent<Rigidbody2D>();
        goombaBC       = GetComponent<BoxCollider2D>();
        goombaAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Wall hit detection.
        wallHit = Physics2D.BoxCast(goombaBC.bounds.center,  goombaBC.bounds.size, 0f, new Vector2(direction, 0), 0.1f, ground).collider != null;

        // Update Goomba movements.
        if (wallHit) direction *= -1;
        goombaRB.velocity = new Vector2(direction * speed * Time.deltaTime, goombaRB.velocity.y);
    }

}
