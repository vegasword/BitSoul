using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovements : MonoBehaviour
{
    private BossHealth  bossHealth;
    private Rigidbody2D bossRB;
    
    public float movementsSpeed            = 60f;
    public float trajectoryCurveComplexity = 2f;
    public float trajectoryRadius          = 5f;

    public Transform bossAnchor;
    public Transform rotationPoint;

    void Start()
    {
        bossHealth = GetComponent<BossHealth>();
        bossRB     = GetComponent<Rigidbody2D>();

        transform.position = rotationPoint.position + Vector3.right * trajectoryRadius;
    }

    void Update()
    {
        if (!bossHealth.isDead)
        {
            // Make the boss rotating around the given rotation point.
            float speed = movementsSpeed * Time.deltaTime;
            float t     = Mathf.Deg2Rad  * Time.frameCount;

            // Rotate the Boss in the opposite direction of the rotation point.
            transform.Rotate(Vector3.forward, -movementsSpeed * Time.deltaTime);

            // The rotation point move following a Lissajous curve according to the anchor set.
            float rx = bossAnchor.position.x + 5f * Mathf.Sin(t / trajectoryCurveComplexity);
            float ry = bossAnchor.position.y + 3f * Mathf.Sin(t);
            rotationPoint.position = new Vector3(rx, ry);

            // The Boss rotate around the rotation point.
            transform.RotateAround(rotationPoint.position, Vector3.forward, speed);
        }
    }
}
