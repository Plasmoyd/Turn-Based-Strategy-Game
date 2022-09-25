#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Controls controls;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        controls = new Controls();
        controls.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM

        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;

#endif
    }

    public bool IsMouseButtonDown()
    {

#if USE_NEW_INPUT_SYSTEM

        return controls.Player.Click.WasPressedThisFrame();

#else
        return Input.GetMouseButtonDown(0);

#endif
    }

    public Vector2 GetCameraMovementDirection()
    {

#if USE_NEW_INPUT_SYSTEM

        return controls.Player.CameraMovement.ReadValue<Vector2>();

#else
        Vector3 inputMoveDirection = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        return inputMoveDirection;

#endif
    }

    public float GetCameraRotationAmount()
    {

#if USE_NEW_INPUT_SYSTEM

        return controls.Player.CameraRotation.ReadValue<float>();

#else
        float rotationAmount = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationAmount = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationAmount = +1f;
        }

        return rotationAmount;

#endif
    }

    public float GetCameraZoomAmount()
    {

#if USE_NEW_INPUT_SYSTEM

        return controls.Player.CameraZoom.ReadValue<float>();

#else
        float zoomamount = 0;

        if(Input.mouseScrollDelta.y > 0)
        {
            zoomamount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomamount = 1f;
        }

        return zoomamount;
#endif
    }
}
