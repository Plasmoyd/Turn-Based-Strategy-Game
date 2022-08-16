using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public abstract class BaseAction : MonoBehaviour
{
    protected bool isActive;
    protected Unit unit;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public abstract String GetActionName();

    public virtual bool IsValidGridPosition(GridPosition gridPosition) => GetValidGridPositionList().Contains(gridPosition);

    public abstract List<GridPosition> GetValidGridPositionList();
}
