using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public HealthBar    playerHealthbar;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerHealth.life = 0;
        playerHealthbar.RefresHealthBar();
    }
}
