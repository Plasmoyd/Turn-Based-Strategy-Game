using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    [SerializeField] private float explosionForce = 300f;
    [SerializeField] private float explosionRadius = 10f;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, transform.position + randomDir, explosionRadius);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform childClone = clone.Find(child.name);

            if(childClone != null)
            {
                childClone.position = child.position;
                childClone.rotation = child.rotation;

                MatchAllChildTransforms(child, childClone); //here i have to use recursion because root is a tree
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach( Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
