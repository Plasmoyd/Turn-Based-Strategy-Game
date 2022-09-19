using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStart;
    public event EventHandler OnSwordActionComplete;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }

    [SerializeField] private int maxSwordDistance = 1;
    [SerializeField] private float beforeHitTimer = 1f;
    [SerializeField] private float afterHitTimer = .5f;
    [SerializeField] int damageAmount = 100;
    [SerializeField] float rotationSpeed = 10f;

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }



        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.SwingingSwordBeforeHit:

                Vector3 targetDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, rotationSpeed * Time.deltaTime);

                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if(stateTimer <= 0)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch(state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                stateTimer = afterHitTimer;
                break;
            case State.SwingingSwordAfterHit:


                OnSwordActionComplete?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x =  - maxSwordDistance; x <=  maxSwordDistance; x++)
        {
            for(int z = - maxSwordDistance; z <=  maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!GridLevel.Instance.IsValidGridPosition(testGridPosition))
                {
                    //this grid position doesn't belong to the grid system.
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    //there is an obstacle on this grid position
                    continue;
                }

                if(!GridLevel.Instance.IsOccupiedGridPosition(testGridPosition))
                {
                    //there are no units on this grid position
                    continue;
                }

                Unit targetUnit = GridLevel.Instance.GetUnitAtGridPosition(testGridPosition);

                if (unit.IsEnemy() == targetUnit.IsEnemy())
                {
                    //friendly fire is not enabled
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetUnit = GridLevel.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        stateTimer = beforeHitTimer;

        this.onActionComplete = onActionComplete;
        OnSwordActionStart?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }

    //this is an animation event
    public void Hit()
    {
        targetUnit.DealDamage(damageAmount);
        OnAnySwordHit?.Invoke(this, EventArgs.Empty);
    }
}
