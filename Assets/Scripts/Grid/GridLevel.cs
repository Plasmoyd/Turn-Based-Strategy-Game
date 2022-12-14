using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{

    public event EventHandler OnAnyUnitMoved;

    [SerializeField] private Transform debugPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    private GridSystem<GridObject> gridSystem;

    public static GridLevel Instance; //Singleton

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one LevelGrid");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(debugPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit) => gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnits();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnit();

    public void ClearUnitAtGridPosition(GridPosition gridPosition, Unit unit) => gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        ClearUnitAtGridPosition(fromGridPosition, unit);
        SetUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMoved?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool IsOccupiedGridPosition(GridPosition gridPosition) => gridSystem.IsOccupiedGridPosition(gridPosition);

    public int GetGridWidth() => gridSystem.GetGridWidth();

    public int GetGridHeight() => gridSystem.GetGridHeight();

    public int GetStartingGridWidth() => gridSystem.GetStartingWidth();

    public int GetStartingGridHeight() => gridSystem.GetStartingHeight();

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetInteractable();

    public void SetInteravtableAtGridPosition(GridPosition gridPosition, IInteractable interactable) => gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);
}
