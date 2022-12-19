using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyCameraController : MonoBehaviour
{
    private bool isBossDezoom;

    private Vector2    offset;

    public float offsetMultiplier =  6f;
    public float smoothSpeed      =  1.45f;
    public float bossDezoomSpeed  =  80f;
    public float bossDezoomTarget = -20f;

    public Transform       target;
    public PlayerMovements playerMovements;

    public UnityEvent bossDezoom;
    
    public Camera cam;

    public void InitPosition(Vector3 pos) { transform.position = pos; }

    void Start()
    {

        if (bossDezoom != null) bossDezoom = new UnityEvent();
        bossDezoom.AddListener(BossDezoomCamera);
    }

    void LateUpdate()
    {
            // Compute current and target position.
            Vector2 currentPosition = new Vector3(transform.position.x, transform.position.y);
            Vector2 targetPosition  = new Vector2(target.position.x, target.position.y);

            // Compute camera x offset according to player direction.
            offset.x = playerMovements.direction * offsetMultiplier;

            // Lerp to the desired position.
            Vector2 desiredPosition  = targetPosition + offset;
            Vector2 smoothedPosition = Vector2.Lerp(currentPosition, desiredPosition, smoothSpeed * Time.deltaTime);
            
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);

            // Boss dezoom trigger.
            if (isBossDezoom && transform.position.z >= bossDezoomTarget)
            {
                float dezoom = bossDezoomSpeed * Time.deltaTime;
                cam.fieldOfView    -= dezoom / 2f;
                transform.position -= new Vector3(0f, 0f, dezoom);
            }
    }

    private void BossDezoomCamera() {  isBossDezoom = true; }
}
