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

    

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;

        UpdateTurnNumberText(TurnSystem.Instance.GetTurnNumber());
    }

    private void UpdateTurnNumberText(int turnNumber)
    {
        turnNumberText.text = "Turn : " + turnNumber;
    }

    public void TurnSystem_OnTurnNumberChanged(object sender, int turnNumber)
    {
        UpdateTurnNumberText(turnNumber);
    }
}
