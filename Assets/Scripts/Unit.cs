using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float stoppingDistance = .1f;

    private Vector3 targetPosition;

    void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized; //without normalizing, we would have a direction vector with a magnitude applied

        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += movementSpeed * Time.deltaTime * moveDirection;
        }
        

        if(Input.GetMouseButtonDown(1))
        {
            SetTargetPosition(MouseWorld.GetMousePosition());
        }
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
