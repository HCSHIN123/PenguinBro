using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public enum bulletType { bullet=0, bomb, threeway, ink }


    [SerializeField]
    protected LayerMask targetMask = 3;
    protected WaitForFixedUpdate wait = new WaitForFixedUpdate();

    protected bool isUsed = false;
    
    virtual public void ReadyToShoot()
    {
    }

    virtual public void Shooting_Physical(Vector3[] _path)
    {
        StartCoroutine(COR_Shoot_Physical(_path));
        GetComponent<Rigidbody>().useGravity = true;
    }
    virtual public IEnumerator COR_Shoot_Physical(Vector3[] _path, float duration = 1.0f) 
    {
        foreach (Vector3 p in _path)
        {
            transform.position = p;
            yield return wait;
        }
    }

    
}

    