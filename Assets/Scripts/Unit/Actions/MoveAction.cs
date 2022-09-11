using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{

    public event EventHandler OnMoveStart;
    public event EventHandler OnMoveStop;

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float stoppingDistance = .1f;

    [SerializeField] int maxMoveDistance = 4;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isActive) return;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized; //without normalizing, we would have a direction vector with a magnitude applied
            transform.position += movementSpeed * Time.deltaTime * moveDirection;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            OnMoveStop?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue; //This grid position does not belong to existing grid system

                if (unitGridPosition == testGridPosition) continue; // Unit is already at this grid position

                if (GridLevel.Instance.IsOccupiedGridPosition(testGridPosition)) continue; // checks if any unit is currently on this grid position. Makes previous check obsolete.

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
    {
        
        this.targetPosition = GridLevel.Instance.GetWorldPosition(targetPosition);

        OnMoveStart?.Invoke(this, EventArgs.Empty);

        ActionStart(onMoveComplete);
    }

    public override string GetActionName() => "Move";

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        int targetsInRange = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetsInRange * 10
        };
    }
}
