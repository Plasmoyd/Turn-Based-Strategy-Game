using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float bulletMovementSpeed = 200f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletExplosionVFXPrefab;

    private Vector3 targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += moveDirection * bulletMovementSpeed * Time.deltaTime; //since at this high speed, bullet can overshoot the enemy, here we are checking if we are overshooting. If we are, then we destroy the bullet. 
                                                                                    // This fixes the issue of going back and forward
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            Instantiate(bulletExplosionVFXPrefab, targetPosition, Quaternion.identity);

            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
