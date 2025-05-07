using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class instructionManager : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject gamepadUI;

    private InputDevice lastDevice;

    void Update()
    {
        var currentDevice = GetCurrentInputDevice();

        if (currentDevice != lastDevice)
        {
            lastDevice = currentDevice;

            if (currentDevice is Gamepad)
            {
                ShowGamepadUI();
            }
            else if (currentDevice is Keyboard || currentDevice is Mouse)
            {
                ShowKeyboardUI();
            }
        }
    }

    InputDevice GetCurrentInputDevice()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device.wasUpdatedThisFrame)
            {
                return device;
            }
        }
        return lastDevice;
    }

    void ShowKeyboardUI()
    {
        keyboardUI.SetActive(true);
        gamepadUI.SetActive(false);
    }

    void ShowGamepadUI()
    {
        keyboardUI.SetActive(false);
        gamepadUI.SetActive(true);
    }
}
