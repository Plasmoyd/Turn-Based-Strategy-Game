using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionComplete;

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

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }
}

