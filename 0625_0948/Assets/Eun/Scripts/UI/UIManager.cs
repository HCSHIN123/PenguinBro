using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ShootCam shootCam;
    [SerializeField] private ShootCanvas shootCanvas;
    [SerializeField] private WorldMap worldMap;

    public void IsTrigger()
    {
        shootCam.ShootOn(Vector3.zero, Vector3.zero);
        shootCanvas.gameObject.SetActive(true);
        shootCanvas.TurnOn();
    }

    public void IsXButton(bool turn)
    {
        worldMap.gameObject.SetActive(turn);
    }
}
