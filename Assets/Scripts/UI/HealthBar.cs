using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    [HideInInspector] public UnityEvent lifeRefreshed;

    public PlayerHealth playerHealth;

    public GameObject heartPrefab;
    public GameObject currentHeartPrefab;
    

    void Start()
    {
        RefresHealthBar();
        
        if (lifeRefreshed == null) lifeRefreshed = new UnityEvent();
        lifeRefreshed.AddListener(RefresHealthBar);
    }

    public void RefresHealthBar()
    {
        foreach(RectTransform child in transform) Destroy(child.gameObject);

        for (int i = 1; i <= playerHealth.life; i++)
        {
            if (i != playerHealth.life)
                Instantiate(heartPrefab, transform);
            else
                Instantiate(currentHeartPrefab, transform);
        }
    }
}
