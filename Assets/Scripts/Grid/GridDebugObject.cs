using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private GridObject gridObject;

    private void Update()
    {
        UpdateText();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    public void UpdateText()
    {
        text.text = gridObject.ToString();
    }
}
