using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public delegate void CallbackMethod();
    private CallbackMethod callbackMethod = null;

    [SerializeField] private ShootCam shootCam;
    [SerializeField] private ShootCanvas shootCanvas;
    [SerializeField] private WorldMap worldMap;
    [SerializeField] private List<RectTransform> playerStatus = null;
    public bool isMyTurn = false;
    private float flowTime = 0f;
    
    public void SetCallbackMethod(CallbackMethod _valueMethod)
    {
        callbackMethod = _valueMethod;
    }

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


    public void StartTurn(float _value)
    {
        isMyTurn = true;
        flowTime = _value;
        StartCoroutine(StartFlowTime());
    }

    private IEnumerator StartFlowTime()
    {

        float originalSize = playerStatus[2].localScale.x;


        while(isMyTurn)
        {            
            originalSize -= flowTime * Time.deltaTime;

            playerStatus[0].localScale = new Vector3(originalSize, 1f, 1f);
            playerStatus[0].GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, originalSize);
            
            yield return new WaitForEndOfFrame();

            if(playerStatus[2].localScale.x <= 0f)
            {
                isMyTurn = false;
                callbackMethod?.Invoke();
            }
            
        }

        playerStatus[0].GetComponent<Image>().color = Color.green;
        playerStatus[0].localScale = Vector3.one;

        yield break;
    }
}
    
