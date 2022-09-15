using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 30;
    [SerializeField] private Transform grenadeExplosionVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    
    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;

        positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance/4;
        float positionY =  arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float targetReachedDistance = .2f;
        
        if( Vector3.Distance(positionXZ, targetPosition) < targetReachedDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.DealDamage(damageAmount);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            Instantiate(grenadeExplosionVFXPrefab, positionXZ, Quaternion.identity);

            trailRenderer.transform.parent = null;

            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = GridLevel.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
