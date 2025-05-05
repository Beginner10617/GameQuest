using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput _playerInput;

    public static Vector2 _movement;
    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpWasReleased;
    public static bool RunIsHeld;
    public static float shootIsHeld;
    public static float mukkaPressed;
    public static bool openDoorPressed;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _shootAction;
    private InputAction _mukkaAction;
    private InputAction _openDoorAction;
    //
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _runAction = _playerInput.actions["Run"];
        _shootAction = _playerInput.actions["Shoot"];
        _mukkaAction = _playerInput.actions["Mukka"];
        _openDoorAction = _playerInput.actions["OpenDoor"];
    }
    // Update is called once per frame
    void Update()
    {
        _movement = _moveAction.ReadValue<Vector2>();

        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpWasReleased = _jumpAction.WasReleasedThisFrame();

        RunIsHeld = _runAction.IsPressed();

        shootIsHeld = _shootAction.ReadValue<float>();
        mukkaPressed = _mukkaAction.ReadValue<float>();

        openDoorPressed = _openDoorAction.ReadValue<float>() > 0.25f;
    }
    public void JumpAction(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Debug.Log("Recognizing jump");
            jumpWasPressed = true;
            jumpWasReleased = false;
        }
        else if (context.canceled)
        {
            jumpWasPressed = false;
            jumpWasReleased = true;
        }
    }
}
