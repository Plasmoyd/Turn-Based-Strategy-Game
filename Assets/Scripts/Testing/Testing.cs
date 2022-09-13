using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = GridLevel.Instance.GetGridPosition(MouseWorld.GetMousePosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> pathList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for(int i = 0; i < pathList.Count - 1; i++)
            {
                
                Debug.DrawLine(GridLevel.Instance.GetWorldPosition(pathList[i]), GridLevel.Instance.GetWorldPosition(pathList[i + 1]), Color.white, 10f);
            }
        }
    }
}
