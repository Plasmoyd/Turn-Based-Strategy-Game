using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionVirtualCamera;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;
    }

    private void EnableActionCamera()
    {
        actionVirtualCamera.SetActive(true);
    }

    private void DisableActionCamera()
    {
        actionVirtualCamera.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:

                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraShoulderHeight = Vector3.up * 1.7f;
                Vector3 shootingDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = .5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootingDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraShoulderHeight + shoulderOffset + (shootingDirection * -1);

                actionVirtualCamera.transform.position = actionCameraPosition;
                actionVirtualCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraShoulderHeight);

                EnableActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:

                DisableActionCamera();
                break;
        }
    }
}
