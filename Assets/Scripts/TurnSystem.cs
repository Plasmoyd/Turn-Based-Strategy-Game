using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public event EventHandler<int> OnTurnNumberChanged;

    private int turnNumber;

    public static TurnSystem Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There is more than one instance of TurnSystem game object");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        OnTurnNumberChanged?.Invoke(this, turnNumber);
    }

    public int GetTurnNumber() => turnNumber;
}
