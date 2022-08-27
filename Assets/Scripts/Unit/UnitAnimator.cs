using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

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

    private void ShootAction_OnShoot(object sender, EventArgs e)
    {
        unitAnimator.SetTrigger("Shoot");
    }
}
