using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    private bool     isBumping = false;
    private Animator bumperAnim;

    public float force;

    public PlayerMovements playerMovements;
    
    void Start()
    {
        bumperAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isBumping && bumperAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            isBumping = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isBumping = true;
            bumperAnim.SetTrigger("Trigger");
            playerMovements.Bounce(force);
        }
    }
}
