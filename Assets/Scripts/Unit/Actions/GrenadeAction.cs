using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private int maxThrowDistance = 7;
    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private float rotationSpeed = 10f;

    private GridPosition targetGridPosition;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        transform.forward = Vector3.Lerp(transform.forward, GridLevel.Instance.GetWorldPosition(targetGridPosition), rotationSpeed * Time.deltaTime); // this needs to be fixed by adding states for aiming and throwing like in shootAction!
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0

        };
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = unitGridPosition.x -  maxThrowDistance; x <= unitGridPosition.x + maxThrowDistance; x++)
        {
            for(int z = unitGridPosition.z - maxThrowDistance; z <= unitGridPosition.z + maxThrowDistance; z++)
            {

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Math.Abs(z);

                //Checks if position is in throwing range
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                //Checks if the position is part of the grid system
                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetGridPosition = gridPosition;

        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
