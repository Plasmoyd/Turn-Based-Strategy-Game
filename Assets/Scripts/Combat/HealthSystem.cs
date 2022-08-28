using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private int healthPoints = 100;

    private int currentHealthPoints;

    private void Awake()
    {
        currentHealthPoints = healthPoints;
    }
    public void DealDamage(int damageAmount)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damageAmount, 0, healthPoints);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        Debug.Log(currentHealthPoints);

        if(currentHealthPoints == 0)
        {
            Die();
        }
    }

    public float GetHealthNormalized()
    {
        return (float)currentHealthPoints / healthPoints;
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
