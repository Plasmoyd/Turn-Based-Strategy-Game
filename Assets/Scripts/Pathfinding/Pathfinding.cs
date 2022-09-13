using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Transform pathfindingDebugObjectTransform;

    private int width = 10;
    private int height = 10;
    private float cellSize = 2f;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(pathfindingDebugObjectTransform);
    }

    public void FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {

    }
}
