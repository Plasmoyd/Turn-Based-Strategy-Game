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
                    
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
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

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {

        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null; 
        

        foreach (BaseAction baseAction in enemyUnit.GetBaseActions())
        {
            if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
                continue;
            }

            EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

            if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
            {
                bestEnemyAIAction = testEnemyAIAction;
                bestBaseAction = baseAction;
            }
        }

        if (bestEnemyAIAction == null || bestBaseAction == null)
        {
            return false;
        }

        if (!enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            return false;
        }

        GridPosition gridPosition = bestEnemyAIAction.gridPosition;
        bestBaseAction.TakeAction(gridPosition, onEnemyAIActionComplete);

        return true;

    }

    private void SetStateTakingTurn()
    {
        timer = .5f;
        state = State.TakingATurn;
    }
}
