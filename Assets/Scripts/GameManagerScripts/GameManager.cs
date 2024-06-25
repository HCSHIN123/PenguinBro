using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerVR playerVR;

    private void Start()
    {
        playerVR.SetVrTriggerdelegate = uiManager.IsTrigger;
        playerVR.SetVrXbuttondelegate = uiManager.IsXButton;
    }
}
