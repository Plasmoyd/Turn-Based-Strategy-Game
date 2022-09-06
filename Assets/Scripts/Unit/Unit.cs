using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;

    [Header("Action points")]
    [SerializeField] private int ACTION_POINTS_PER_TURN = 4;
    [SerializeField] private int currentActionPoints = 4;

    [Header("Is Enemy")]
    [SerializeField] private bool isEnemy;

    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActions;
    private HealthSystem healthSystem;

    

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;

        currentGridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.SetUnitAtGridPosition(currentGridPosition, this);

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    void Update()
    {

        GridPosition newGridPosition = GridLevel.Instance.GetGridPosition(transform.position);

        if(currentGridPosition != newGridPosition)
        {
            GridPosition oldGridPosition = currentGridPosition;
            currentGridPosition = newGridPosition;

            GridLevel.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition); //it's important that this line is after assigning new grid position
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

    public Vector3 GetWorldPosition() => transform.position;

    public BaseAction[] GetBaseActions() => baseActions;

    public int GetCurrentActionPoints() => currentActionPoints;

    public bool IsEnemy() => isEnemy;

    private void SpendActionPoints(int actionPoints)
    {
        this.currentActionPoints -= actionPoints;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DealDamage(int damageAmount)
    {
        healthSystem.DealDamage(damageAmount);
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, int turnNumber)
    {
        if(!((TurnSystem.Instance.IsPlayerTurn() && !isEnemy) || (!TurnSystem.Instance.IsPlayerTurn() && isEnemy)))
        {
            return; // basically this method should execute only if it's the players turn and it's not an enemy unit or if it's an enemy turn and it's an enemy unit. If it's not that, we return
        }

        currentActionPoints = ACTION_POINTS_PER_TURN;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        GridLevel.Instance.ClearUnitAtGridPosition(currentGridPosition, this);
        Destroy(gameObject);
    }

}
