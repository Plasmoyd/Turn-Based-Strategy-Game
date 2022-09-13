using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost() => gCost;

    public int GetHCost() => hCost;

    public int GetFCost() => fCost;

    public PathNode GetCameFromPathNode() => cameFromPathNode;

    public GridPosition GetGridPosition() => gridPosition;

    public bool IsWalkable() => isWalkable;

    public void SetGCost(int gCost) => this.gCost = gCost;

    public void SetHCost(int hCost) => this.hCost = hCost;

    public void CalculateFCost() => fCost = gCost + hCost;

    public void SetCameFromPathNode(PathNode cameFromPathNode) => this.cameFromPathNode = cameFromPathNode;

    public void ResetCameFromPathNode() => cameFromPathNode = null;

    public void SetIsWalkable(bool isWalkable) => this.isWalkable = isWalkable;

}
