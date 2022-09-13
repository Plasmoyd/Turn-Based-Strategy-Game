using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro gridPositionText;

    private object gridObject;

    private void Update()
    {
        UpdateText();
    }

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void UpdateText()
    {
        gridPositionText.text = gridObject.ToString();
    }
}
