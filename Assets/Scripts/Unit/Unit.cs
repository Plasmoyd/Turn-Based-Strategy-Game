using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
    }

    private void Start()
    {
        currentGridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.SetUnitAtGridPosition(currentGridPosition, this);
    }

    void Update()
    {

        GridPosition newGridPosition = GridLevel.Instance.GetGridPosition(transform.position);

        if(currentGridPosition != newGridPosition)
        {
            GridLevel.Instance.ClearUnitAtGridPosition(currentGridPosition, this);
            GridLevel.Instance.SetUnitAtGridPosition(newGridPosition, this);
            currentGridPosition = newGridPosition;
        }
        
    }


    public MoveAction GetMoveAction() => moveAction;

    public SpinAction GetSpinAction() => spinAction;

    public GridPosition GetGridPosition() => currentGridPosition;
}
