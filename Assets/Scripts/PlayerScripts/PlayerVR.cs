using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerVR : MonoBehaviour
{
    public delegate void vrTriggerdelegate();
    private vrTriggerdelegate VrTriggerdelegate = null;
    public vrTriggerdelegate SetVrTriggerdelegate { set { VrTriggerdelegate = value; } }

    public delegate void vrXbuttondelegate(bool turn);
    private vrXbuttondelegate VrXbuttondelegate = null;
    public vrXbuttondelegate SetVrXbuttondelegate { set { VrXbuttondelegate = value; } }

    [SerializeField] private Canvas statusCanva;
    private Slider[] statusSlider;
    public Sprite sp;
    
    public InputActionProperty[] inputAction; // RightTrigger = 0, LeftXbutton = 1, LeftJoyStick = 2;
    public ContinuousMoveProviderBase move;
    private bool isPressed = false;
    private bool coLoop = false;

    private int hp = 10;
    private float mp = 1000;

    private void Start()
    {
        inputAction[1].action.performed += XbuttonPress;
        move = GetComponent<ContinuousMoveProviderBase>();
        statusSlider = statusCanva.GetComponentsInChildren<Slider>();

        SetCouroutine(true);
        SetHp(1);
    }

    private void XbuttonPress(InputAction.CallbackContext isPress)
    {
        VrXbuttondelegate(!isPressed);
    }

    private IEnumerator inputCoroutine()
    {

        while(coLoop)
        {

            float triggerActionValue = inputAction[0].action.ReadValue<float>();

            if (triggerActionValue >= 0.9f)
            {
                VrTriggerdelegate();
            }

            float joyStickValuey = inputAction[2].action.ReadValue<Vector2>().y;

            if (joyStickValuey != 0)
            {
                mp -= Mathf.Abs(joyStickValuey);
                Debug.Log("mp : " + mp);
                float scale = (float)(mp * 0.0005);
                statusSlider[0].value = scale;
            }

            if (mp <= 0)
            {
                move.enabled = false;
            }

            yield return null;
        }

    }

    public void SetCouroutine(bool setBool)
    {
        coLoop = setBool;
        if (coLoop)
        {
            StartCoroutine("inputCoroutine");
        }
        else
        {
            StopCoroutine("inputCoroutine");
        }
    }

    public void SetHp(int damage)
    {
        hp -= damage;
        Debug.Log("HP : " + hp);
        if (hp <= 0)
        {
            Debug.Log("Dead");
        }

        else
        {
            float scaleCanva = statusSlider[1].value;
            float length = scaleCanva - (scaleCanva * (float)(damage * 0.1));
            scaleCanva = length;
            statusSlider[1].value = scaleCanva;
        }
    }
}
