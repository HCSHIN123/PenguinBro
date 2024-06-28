using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Bullet_ThreeWay : Bullet_DistributedAttack
{
    [SerializeField]
    private float splitSpeed = 5f;
    
    private void Awake()
    {
        bulletTransforms = new Transform[2];
        bulletTransforms[0] = transform.GetChild(0).transform;
        bulletTransforms[1] = transform.GetChild(1).transform;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            isSplit = true;
    }

    public override void ReadyToShoot()
    {
        base.ReadyToShoot();
    }
    
    override public IEnumerator COR_Shoot_Physical(Vector3[] _path, float duration = 1.0f)
    {
        
        float splitTime = 0f;
        foreach(Transform t in bulletTransforms)
        {
            t.GetComponent<Rigidbody>().useGravity = true;
        }
        foreach (Vector3 p in _path)
        {
            if (isHited)
                break;
            if (isSplit)
            {
                splitTime += Time.deltaTime;
                Vector3 moveDirection = p - transform.position;
                moveDirection.Normalize();
                Vector3 leftdir = Quaternion.Euler(0f, -90f, 0f) * moveDirection;
                Vector3 rightdir = Quaternion.Euler(0f, 90f, 0f) * moveDirection;
                leftdir.Normalize();
                rightdir.Normalize();

                bulletTransforms[0].localPosition = splitTime * 0.01f * splitSpeed * Vector3.left;
                bulletTransforms[1].localPosition = splitTime * 0.01f *splitSpeed * Vector3.right;
            }
            transform.LookAt(p);
            transform.position = p;
            yield return wait;
        }
    }

    protected override void OnCollisionBulletEvent(Collision collision)
    {
        if (collision.collider.CompareTag(breakableTag))
            StartCoroutine(COR_BulletOff());
    }
}

    //override public IEnumerator COR_Shoot(float duration = 1.0f)
    //{
    //    float time = 0f;
    //    float splitTime = 0f;

    //    while (time <= 1f)
    //    {
    //        Vector3 p4 = Vector3.Lerp(start, peak, time);
    //        Vector3 p5 = Vector3.Lerp(peak, end, time);
    //        transform.position = Vector3.Lerp(p4, p5, time);
    //        if (isSplit)
    //        {
    //            splitTime += Time.deltaTime;
    //            //Vector3 leftpos = new Vector3(transform.position.x - (splitSpeed * splitTime), transform.position.y, transform.position.z);
    //            //Vector3 rightpos = new Vector3(transform.position.x + (splitSpeed * splitTime), transform.position.y, transform.position.z);
    //            //left.position = Vector3.Lerp(transform.position, leftpos, time);
    //            //right.position = Vector3.Lerp(transform.position, rightpos, time);                
    //            Vector3 leftpos =  new Vector3(transform.position.x - (splitSpeed * splitTime), transform.position.y, transform.position.z);
    //            Vector3 rightpos = new Vector3(transform.position.x + (splitSpeed * splitTime), transform.position.y, transform.position.z);
    //            left.position = Vector3.Lerp(transform.position, leftpos, time);
    //            right.position = Vector3.Lerp(transform.position, rightpos, time);
    //        }
    //        time += Time.deltaTime / (duration * range * 0.02f);
    //        yield return null;
    //    }

    //    isShooting = false;
    //}