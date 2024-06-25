using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum eBulletType
    {
        NORMAL, BOMB, SPLIT_3,
    }


    [SerializeField]
    protected float range = 30f;
    protected bool isShooting = false;
    protected Vector3 start, peak, end;
    protected WaitForFixedUpdate wait = new WaitForFixedUpdate();
    public float Range { get { return range; } }
    private void Awake()
    {
        Init();
    }
    virtual public void Init()
    {
        isShooting = false;
        start = Vector3.zero;
        peak = Vector3.zero;
        end = Vector3.zero;
}

    virtual public void Shooting(Transform _start, Transform _peak, Transform _end)
    {
        if (isShooting)
            return;
        isShooting=true;
        start = _start.position;
        peak = _peak.position;
        end = _end.position;
        StartCoroutine(COR_Shoot());
    }

    virtual public IEnumerator COR_Shoot(float duration = 1.0f)
    {
        float time = 0f;

        while (time <= 1f)
        {
            Vector3 p4 = Vector3.Lerp(start, peak, time);
            Vector3 p5 = Vector3.Lerp(peak, end, time);
            transform.position = Vector3.Lerp(p4, p5, time);
            time += Time.deltaTime / (duration * range * 0.02f);
            yield return null;
        }

        isShooting = false;
    }
    virtual public void Shooting_Physical(Vector3[] _path)
    {
        StartCoroutine(COR_Shoot_Physical(_path));
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
