using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{

    [SerializeField] private int maxInteractDistance = 1;


    private void Update()
    {
        if(!isActive)
        {
            return;
        }

    }

    public override string GetActionName()
    {
        return "Interact";
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

        for(int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for(int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unit.GetGridPosition() + offsetGridPosition;

                if(!GridLevel.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

               IInteractable interactable = GridLevel.Instance.GetDoorAtGridPosition(testGridPosition);
                if(interactable == null)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = GridLevel.Instance.GetDoorAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }
}
