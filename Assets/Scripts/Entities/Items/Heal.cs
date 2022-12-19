using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private bool  hasHealed;

    public PlayerHealth playerHealth;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasHealed && playerHealth.life < playerHealth.maxLives)
        {
            playerHealth.playerHealed.Invoke();
            hasHealed = true;
            Destroy(this.gameObject);
        }
    }
}