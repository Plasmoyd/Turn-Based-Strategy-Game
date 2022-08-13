using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool isBusy;

    public event EventHandler OnSelectedUnitChanged;

    public static UnitActionSystem Instance { get; private set; } //Singleton

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Error! There are multiple instances of UnitActionSystem :" + this + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleUnitSelection();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetSelectedUnitsTargetPosition();
            SetBusy();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            HandleSpinning();
            SetBusy();
        }
    }

    private void HandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
            }
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedUnitsTargetPosition()
    {
        GridPosition mouseGridPosition = GridLevel.Instance.GetGridPosition(MouseWorld.GetMousePosition()); // turning mice world position to corresponding grid position

        if(GetSelectedUnit().GetMoveAction().IsValidGridPosition(mouseGridPosition)) // checking whether that gridPosition is valid
        {
            selectedUnit.GetMoveAction().SetTargetPosition(mouseGridPosition, ClearBusy); //if it is, then we setSelected units target position
        }

    }

    private void HandleSpinning() => GetSelectedUnit().GetSpinAction().StartSpinning(ClearBusy);

    private void SetBusy() => isBusy = true;

    private void ClearBusy() => isBusy = false;

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
