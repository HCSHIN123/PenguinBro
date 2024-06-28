using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum bulletType { bullet = 0, bomb, threeway, ink }
    private Vector3 startPos = Vector3.zero;
    private Quaternion startRot = Quaternion.identity;

    [SerializeField]
    protected LayerMask targetMask = 3;
    [SerializeField]
    protected float endTime = 0.0f;

    protected WaitForFixedUpdate wait = new WaitForFixedUpdate();
    protected bool isHited = false;
    protected string breakableTag = "Breakable";
    [SerializeField]
    protected Rigidbody rb;

    private void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        rb = GetComponent<Rigidbody>();

    }

    virtual public void ReadyToShoot()
    {
        gameObject.SetActive(true);
        isHited = false;
        transform.localPosition = startPos;
        transform.localRotation = startRot;
       
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.isKinematic = false;
    }

    virtual public void Shooting_Physical(Vector3[] _path)
    {
        StartCoroutine(COR_Shoot_Physical(_path));
        rb.useGravity = true;
    }
    virtual public IEnumerator COR_Shoot_Physical(Vector3[] _path, float duration = 1.0f) 
    {
        foreach (Vector3 p in _path)
        {
            if (isHited)
                break;
            transform.LookAt(p);
            transform.position = p;
            yield return wait;
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        OnCollisionBulletEvent(collision);
    }

    virtual protected void OnCollisionBulletEvent(Collision collision)
    {
        if (collision.gameObject.CompareTag(breakableTag))
        {
            isHited = true;
            StartCoroutine(COR_BulletOff());
        }
    }

    virtual protected IEnumerator COR_BulletOff()
    {
        if(endTime == 0f)
            this.gameObject.SetActive(false);
        else
        {
            yield return new WaitForSecondsRealtime(endTime);
            this.gameObject.SetActive(false);
        }

        
    }
}

    