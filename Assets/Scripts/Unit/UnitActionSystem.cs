using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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
        if (Input.GetMouseButtonDown(0))
        {
            HandleUnitSelection();
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit.SetTargetPosition(MouseWorld.GetMousePosition());
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

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
