using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft
    }

    [SerializeField] Transform gridSystemVisualSinglePrefab;
    [SerializeField] List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] visualSingleArray;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Error! There are multiple instances of GridSystemVisual :" + this + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        GridLevel.Instance.OnAnyUnitMoved += GridLevel_OnAnyUnitMoved;
    }

    private void UpdateGridVisuals()
    {
        HideAllGridPositions();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType;
        GridPosition gridPosition;

        switch (selectedAction)
        {
            default:

            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:

                gridVisualType = GridVisualType.Red;

                gridPosition = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
                int range = shootAction.GetMaxShootDistance();
                ShowGridPositionRange(gridPosition, range, GridVisualType.RedSoft);

                break;

            case GrenadeAction grenadeAction:

                gridVisualType = GridVisualType.Yellow;

                break;

            case SwordAction swordAction:

                gridPosition = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
                gridVisualType = GridVisualType.Red;
                int swordRange = swordAction.GetMaxSwordDistance();
                ShowGridPositionRangeSquare(gridPosition, swordRange, gridVisualType);

                break;

            case InteractAction interactAction:

                gridVisualType = GridVisualType.Blue;

                break;
        }

        ShowGridPositionList(selectedAction.GetValidGridPositionList(), gridVisualType);

    }

    public void HideAllGridPositions()
    {
        foreach(GridSystemVisualSingle visualSingle in visualSingleArray)
        {
            visualSingle.Hide();
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for(int x =  - range; x <=  range; x++)
        {
            for(int z =  - range; z <=  + range; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue; //Makes it so our range isnt a very big square, but a diamond shaped


                gridPositionList.Add(testGridPosition);
                
            }
        }

        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= +range; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;

                gridPositionList.Add(testGridPosition);

            }
        }

        ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {

        foreach(GridPosition gridPosition in gridPositionList)
        {
            visualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void GridLevel_OnAnyUnitMoved(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.Log("Error : There is no material assigned to this GridVisualType : " + gridVisualType);
        return null;
    }

}
