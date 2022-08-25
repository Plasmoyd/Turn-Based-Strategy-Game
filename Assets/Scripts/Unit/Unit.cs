using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private const int ACTION_POINTS_PER_TURN = 4;
    [SerializeField] private int currentActionPoints = 4;

    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActions;

    public static event EventHandler OnAnyActionPointsChanged;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;

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

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => currentActionPoints >= baseAction.GetActionPointsCost();

    
    public MoveAction GetMoveAction() => moveAction;

    public SpinAction GetSpinAction() => spinAction;

    public GridPosition GetGridPosition() => currentGridPosition;

    public BaseAction[] GetBaseActions() => baseActions;

    public int GetCurrentActionPoints() => currentActionPoints;

    private void SpendActionPoints(int actionPoints)
    {
        this.currentActionPoints -= actionPoints;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, int turnNumber)
    {
        currentActionPoints = ACTION_POINTS_PER_TURN;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
}
