using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [SerializeField] private int maxShootDistance = 5;
    [SerializeField] private float aimingTimer = 2f;
    [SerializeField] private float shootingTimer = .2f;
    [SerializeField] private float cooloffTimer = .2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int damageAmount = 40;
    [SerializeField] private LayerMask obstacleLayerMask;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private Unit targetUnit;
    private State state;
    private float stateTimer;
    private bool canShootBullet;
    

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:

                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, shootDirection, Time.deltaTime * rotationSpeed);

                break;
            case State.Shooting:

                if (!canShootBullet) break;

                Shoot();

                break;
            case State.Cooloff:
                break;
        }

        stateTimer -= Time.deltaTime;

        if(stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.DealDamage(damageAmount);
        canShootBullet = false;
    }

    private void NextState()
    {
        switch(state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = shootingTimer;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = cooloffTimer;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }


    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {

        GridPosition unitGridPositon = unit.GetGridPosition();
        return GetValidGridPositionList(unitGridPositon);
    }

    public  List<GridPosition> GetValidGridPositionList(GridPosition gridPosition)
    {

        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = gridPosition;

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxShootDistance) continue; //Makes it so our range isnt a veru big square, but a diamond shaped

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue; //this grid position does not belong to the grid system

                //if (unitGridPosition == testGridPosition) continue; // we dont want unit to shoot itself. This line became obsolete, but will matter in case we want friendly fire for some reason in the future.

                if (!GridLevel.Instance.IsOccupiedGridPosition(testGridPosition)) continue; //we dont want player to be able to shoot empty space

                Unit targetUnit = GridLevel.Instance.GetUnitAtGridPosition(testGridPosition);

                //No need for null  check here because i'm checking wheter there's a unit on this position in previous check
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue; //if target and unit are on the same team, we don't want friendly fire.

                float unitShoulderHeight = 1.7f;
                Vector3 startPosition = unit.GetWorldPosition() + Vector3.up * unitShoulderHeight;
                Vector3 direction = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                bool obstacleHit = Physics.Raycast(startPosition, direction, Vector3.Distance(unit.GetWorldPosition(), targetUnit.GetWorldPosition()), obstacleLayerMask);

                if (obstacleHit) continue; //we don't want to shoot through obstacles

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onShootComplete)
    {
        
        this.targetUnit = GridLevel.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        state = State.Aiming;
        stateTimer = aimingTimer;

        ActionStart(onShootComplete);

    }

    public Unit GetTargetUnit() => targetUnit;

    public int GetMaxShootDistance() => maxShootDistance;

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        Unit targetUnit = GridLevel.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) *100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList(gridPosition).Count;
    }
}
