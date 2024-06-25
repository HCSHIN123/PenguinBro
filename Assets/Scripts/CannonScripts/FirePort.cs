using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePort : MonoBehaviour
{
    [SerializeField]
    private Transform firePort;

    private Vector3 dir = Vector3.zero;
    public Vector3 Dir 
    {
        get 
        {
            dir = firePort.position - transform.position;
            dir.Normalize();
            return dir;
        }
    }
}
