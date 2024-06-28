using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePort : MonoBehaviour
{
    public Transform bulletPos;

    private Vector3 dir = Vector3.zero;
    public Vector3 Dir 
    {
        get 
        {
            dir = bulletPos.position - transform.position;
            dir.Normalize();
            return dir;
        }
    }
}
