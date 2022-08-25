using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualUI;
    

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;

        UpdateTurnNumberText(TurnSystem.Instance.GetTurnNumber());
        UpdateEnemyTurnVisualUI();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnNumberText(int turnNumber)
    {
        turnNumberText.text = "Turn : " + turnNumber;
    }

    private void UpdateEnemyTurnVisualUI()
    {
        enemyTurnVisualUI.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn()); 
    }

    public void TurnSystem_OnTurnNumberChanged(object sender, int turnNumber)
    {
        UpdateTurnNumberText(turnNumber);
        UpdateEnemyTurnVisualUI();
        UpdateEndTurnButtonVisibility();
    }
}
