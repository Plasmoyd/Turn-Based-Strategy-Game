using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{

    [SerializeField] Transform debugPrefab;

    private GridSystem gridSystem;

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

        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugObjects(debugPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit) => gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnits();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).GetUnit();

    public void ClearUnitAtGridPosition(GridPosition gridPosition, Unit unit) => gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool IsOccupiedGridPosition(GridPosition gridPosition) => gridSystem.IsOccupiedGridPosition(gridPosition);

    public int GetGridWidth() => gridSystem.GetGridWidth();

    public int GetGridHeight() => gridSystem.GetGridHeight();

    public int GetStartingGridWidth() => gridSystem.GetStartingWidth();

    public int GetStartingGridHeight() => gridSystem.GetStartingHeight();

}
