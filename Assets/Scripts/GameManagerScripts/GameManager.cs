using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerVR playerVR;        
    [SerializeField] private float timeLimit = 0f;
    [SerializeField] private float timeFlowSpeed = 1f;
    [SerializeField] private GameObject[] cannons = null;
    // [SerializeField] private List<Transform> tragetObj = null;
 

    private void Awake()
    {
        // playerVR.SetVrTriggerdelegate = uiManager.IsTrigger;
        // playerVR.SetVrXbuttondelegate = uiManager.IsXButton;
        uiManager.SetCallbackMethod(StartGame);
        foreach(GameObject cannon in cannons)
        {
            cannon.GetComponentInChildren<Cannon>().SetCallbackMethod(StartTurn);
        }
        cannons[1].GetComponentInChildren<Cannon>().isMyTurn = true;
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {           
            StartTurn();    
            
        }
    }
    private void StartGame()
    {
        foreach(GameObject g in cannons)
        {
            g.GetComponentInChildren<Cannon>().isMyTurn = !g.GetComponentInChildren<Cannon>().isMyTurn;
            

            if(g.GetComponentInChildren<Cannon>().isMyTurn) 
                g.GetComponentInChildren<Cannon>().StartShooting();
        }
    }

    private void StartTurn()
    {
       uiManager.StartTurn(timeFlowSpeed);
    }
    



    
    


}
