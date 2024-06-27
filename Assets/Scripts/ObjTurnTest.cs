using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTurnTest : MonoBehaviour
{
    public delegate void CallbackMethod(string _value);
    private CallbackMethod callbackMethod = null;
    private MeshRenderer meshRenderer = null;
    public bool isMyTurn = false;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void StartObjColor()
    {
        StartCoroutine(showTimeColor());
    }

    public void SetCallbackMethod(CallbackMethod _targetMethod)
    {
        callbackMethod = _targetMethod;
    }

    private IEnumerator showTimeColor()
    {
        while(isMyTurn)
        {
            meshRenderer.material.color = new Color(
                Random.Range(0f,255f)/255f,
                Random.Range(0f,255f)/255f,
                Random.Range(0f,255f)/255f);

            yield return null;
        }

        // if(!isMyTurn) callbackMethod?.Invoke(gameObject.name);
        yield break;
    }
}
