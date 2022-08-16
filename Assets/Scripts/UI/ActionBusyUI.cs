using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private bool isBusy;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        UpdateBusyVisual();
    }

    public void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        this.isBusy = isBusy;
        UpdateBusyVisual();
    }

    private void UpdateBusyVisual()
    {
        this.gameObject.SetActive(isBusy);
    }
}
