using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinAction : BaseAction
{

    [SerializeField] int actionPointCost = 2;

    private float totalSpinAmount;


    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        Spin();
    }

    private void Spin()
    {
        float spinAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if (totalSpinAmount >= 360)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        totalSpinAmount = 0f;

        ActionStart(onSpinComplete);
    }

    public override string GetActionName() => "Spin";

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition gridPosition = unit.GetGridPosition();
        validGridPositionList.Add(gridPosition);

        return validGridPositionList;
    }

    public override int GetActionPointsCost()
    {
        return actionPointCost;
    }
}
