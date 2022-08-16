using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] visualSingleArray;

    private void Start()
    {
        InstantiateGridVisuals();
    }


    private void Update()
    {
        UpdateGridVisuals();
    }

    private void InstantiateGridVisuals()
    {
        visualSingleArray = new GridSystemVisualSingle[GridLevel.Instance.GetGridWidth(), GridLevel.Instance.GetGridHeight()];

        for (int x = GridLevel.Instance.GetStartingGridWidth(); x < GridLevel.Instance.GetGridWidth(); x++)
        {
            for (int z = GridLevel.Instance.GetStartingGridHeight(); z < GridLevel.Instance.GetGridHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform visualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, GridLevel.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                GridSystemVisualSingle visualSingle = visualSingleTransform.GetComponent<GridSystemVisualSingle>();
                if (visualSingle == null) continue;

                visualSingleArray[x, z] = visualSingle;
            }
        }
    }

    private void UpdateGridVisuals()
    {
        HideAllGridPositions();
        ShowGridPositionList(UnitActionSystem.Instance.GetSelectedAction().GetValidGridPositionList());

    }

    public void HideAllGridPositions()
    {
        foreach(GridSystemVisualSingle visualSingle in visualSingleArray)
        {
            visualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {

        foreach(GridPosition gridPosition in gridPositionList)
        {
            visualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    

}
