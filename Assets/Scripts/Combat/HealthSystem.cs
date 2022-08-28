using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDead;

    [SerializeField] private int healthPoints = 100;

    private int currentHealthPoints;

    private void Awake()
    {
        currentHealthPoints = healthPoints;
    }
    public void DealDamage(int damageAmount)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damageAmount, 0, healthPoints);

        Debug.Log(currentHealthPoints);

        if(currentHealthPoints == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
