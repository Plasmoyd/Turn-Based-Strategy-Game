using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{

    public static event EventHandler OnAnyCrateDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private float explosionRadius = 1f;
    

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = GridLevel.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(crateDestroyedTransform, explosionForce, transform.position, explosionRadius);

        Destroy(gameObject);
        OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
