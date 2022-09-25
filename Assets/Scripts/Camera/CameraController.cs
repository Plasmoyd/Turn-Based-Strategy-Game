using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    private const float MIN_FOLLOW_Y_OFFSET = 2f;

    [SerializeField] float cameraMovementSpeed = 10f;
    [SerializeField] float cameraRotationSpeed = 10f;
    [SerializeField] float zoomSpeed = 100f;
    [SerializeField] float zoomAmount = 2f;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 followOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMovementDirection();


        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
        transform.position += moveVector * cameraMovementSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotationAmount();

        transform.eulerAngles += rotationVector * cameraRotationSpeed * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {

        followOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomAmount;

        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
