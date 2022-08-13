using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinAction : BaseAction
{

    private float totalSpinAmount;


    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        Spin();
    }

    private void Spin()
    {
        float spinAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if (totalSpinAmount >= 360)
        {
            isActive = false;
            onActionComplete();
        }
    }

    public void StartSpinning(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }
}
