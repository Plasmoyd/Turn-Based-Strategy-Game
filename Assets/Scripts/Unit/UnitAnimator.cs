using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMoveStart += MoveAction_OnMoveStart;
            moveAction.OnMoveStop += MoveAction_OnMoveStop;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnMoveStart(object sender, EventArgs e)
    {
        unitAnimator.SetBool("isRunning", true);
    }

    private void MoveAction_OnMoveStop(object sender, EventArgs e)
    {
        unitAnimator.SetBool("isRunning", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        unitAnimator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetPosition = e.targetUnit.GetWorldPosition();
        targetPosition.y = shootPointTransform.position.y; // this fixes the issue where unit was shooting at another units feet. this makes sure that a bullet will fly horizontally.

        bulletProjectile.Setup(targetPosition);
    }
}