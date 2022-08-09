using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float stoppingDistance = .1f;
    [SerializeField] Animator unitAnimator;

    private Vector3 targetPosition;

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
