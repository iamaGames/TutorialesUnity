using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputCameraController : MonoBehaviour
{
    private GamepadControls controls;
    private Vector2 LookDelta;

    private void Awake()
    {
        controls = new GamepadControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisname)
    {
        LookDelta = controls.Gameplay.Rotate.ReadValue<Vector2>();

        if (axisname == "Mouse X")
        {
            return LookDelta.x;
        }
        else if (axisname == "Mouse Y")
        {
            return LookDelta.y;
        }
        return 0;
    }
}

