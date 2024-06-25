using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using static PlayerVR;

public class PlayerVR : MonoBehaviour
{
    public delegate void vrTriggerdelegate();
    private vrTriggerdelegate VrTriggerdelegate = null;
    public vrTriggerdelegate SetVrTriggerdelegate { set { VrTriggerdelegate = value; } }

    public delegate void vrXbuttondelegate(bool turn);
    private vrXbuttondelegate VrXbuttondelegate = null;
    public vrXbuttondelegate SetVrXbuttondelegate { set { VrXbuttondelegate = value; } }


    public InputActionProperty triggerAction;
    public InputActionProperty LxButton;
    private bool isPressed = false;

    private void Update()
    {

        float triggerActionValue = triggerAction.action.ReadValue<float>();

        if (triggerActionValue >= 0.9f)
        {
            VrTriggerdelegate();
        }

        bool LxButtonValue = LxButton.action.ReadValue<bool>();

        if (LxButtonValue)
        {
            LxButtonValue = false;
            isPressed = !isPressed;
            VrXbuttondelegate(isPressed);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isPressed = !isPressed;
            VrXbuttondelegate(isPressed);
        }
        if (Input.GetMouseButtonDown(1))
        {
            VrTriggerdelegate();
        }
    }
}
