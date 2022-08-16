using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] Transform actionButtonPrefab;
    [SerializeField] Transform actionButtonContainerTransform;
    [SerializeField] TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonList;

    private void Awake()
    {
        actionButtonList = new List<ActionButtonUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;

        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPointsUI();
    }

    private void CreateUnitActionButtons()
    {
        ClearButtons();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButton = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButton.SetBaseAction(baseAction);

            actionButtonList.Add(actionButton);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPointsUI();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPointsUI();
    }

    private void ClearButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonList.Clear();
    }

    private void UpdateSelectedVisuals()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        foreach(ActionButtonUI actionButton in actionButtonList)
        {
            actionButton.UpdateSelectedVisual(selectedAction);
        }
    }

    private void UpdateActionPointsUI()
    {
        actionPointsText.text = "Action points: " + UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
    }
}
