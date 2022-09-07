using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    private enum State
    {
        WaitionOnEmemyTurn,
        TakingATurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitionOnEmemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
    }

    private void Update()
    {
        
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch(state)
        {
            case State.WaitionOnEmemyTurn:

                break;

            case State.TakingATurn:

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = State.WaitionOnEmemyTurn;
                    TurnSystem.Instance.NextTurn();
                }

                break;

            case State.Busy:

                break;
        }
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, int turnNumber)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingATurn;
            timer = 2f;
        }
        
    }
}
