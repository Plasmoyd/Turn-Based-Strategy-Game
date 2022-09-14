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

    private const int pathfindingDistanceMultiplier = 10;

    private List<Vector3> positionList;
    private int currentPositionIndex;


    void Update()
    {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized; //without normalizing, we would have a direction vector with a magnitude applied

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            
            transform.position += movementSpeed * Time.deltaTime * moveDirection;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            currentPositionIndex++;

            if(currentPositionIndex >= positionList.Count)
            {
                OnMoveStop?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }

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

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue; // this position contains an obstacle of some kind

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)) continue; // doesn't have a path to the node

                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > (maxMoveDistance * pathfindingDistanceMultiplier)) continue; //we want the distance to be pathfinding distance, not distance through walls

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
    {
        List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition gridPosition in gridPositionList)
        {
            positionList.Add(GridLevel.Instance.GetWorldPosition(gridPosition));
        }

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
