using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MoveAction : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float stoppingDistance = .1f;
    [SerializeField] Animator unitAnimator;

    [SerializeField] int maxMoveDistance = 4;

    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        targetPosition = transform.position;
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized; //without normalizing, we would have a direction vector with a magnitude applied
            transform.position += movementSpeed * Time.deltaTime * moveDirection;

            unitAnimator.SetBool("isRunning", true);
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            unitAnimator.SetBool("isRunning", false);
        }
    }

    public List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue; //This grid position does not belong to existing grid system

                if (unitGridPosition == testGridPosition) continue; // Unit is already at this grid position

                if (GridLevel.Instance.IsOccupiedGridPosition(testGridPosition)) continue; // checks if any unit is currently on this grid position. Makes previous check obsolete.

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => GetValidGridPositionList().Contains(gridPosition);

    public void SetTargetPosition(GridPosition targetPosition) => this.targetPosition = GridLevel.Instance.GetWorldPosition(targetPosition);
    
}
