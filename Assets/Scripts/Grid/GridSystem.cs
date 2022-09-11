using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{

    private const int STARTING_WIDTH = 0;
    private const int STARTING_HEIGHT = 0;

    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridObjects;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        this.gridObjects = new TGridObject[width, height];

        for(int x = STARTING_WIDTH; x < width; x++)
        {
            for(int z = STARTING_HEIGHT; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(

            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition) as GridObject);
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition) => gridObjects[gridPosition.x, gridPosition.z];

    public bool IsValidGridPosition(GridPosition gridPosition) => (gridPosition.x >= STARTING_WIDTH && gridPosition.x < width) && (gridPosition.z >= STARTING_HEIGHT && gridPosition.z < height);

    public bool IsOccupiedGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridObject(gridPosition) as GridObject;
        return gridObject.IsPopulated();
    }//checks if there are any units stored in grid object on this grid position

    public int GetGridWidth() => this.width;

    public int GetGridHeight() => this.height;

    public int GetStartingWidth() => STARTING_WIDTH;

    public int GetStartingHeight() => STARTING_HEIGHT;

}
