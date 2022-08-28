using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class WorldUnitUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetCurrentActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
