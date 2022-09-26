using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        unitList = new List<Unit>();
    }

    public override string ToString()
    {

        string unitString = "";
        foreach(Unit unit in unitList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public List<Unit> GetUnits() => unitList;

    public void AddUnit(Unit unit) => this.unitList.Add(unit);

    public void RemoveUnit(Unit unit) => this.unitList.Remove(unit);

    public bool IsPopulated() => this.unitList.Count > 0;

    public Unit GetUnit()
    {
        return IsPopulated() ? unitList[0] : null; 
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }
}
