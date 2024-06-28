using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet_DistributedAttack : Bullet
{
    [SerializeField]
    protected Transform[] bulletTransforms;
    protected bool isSplit = false;

    public override void ReadyToShoot()
    {
        foreach (Transform t in bulletTransforms)
        {
            t.GetComponent<Rigidbody>().useGravity = false;
        }
        base.ReadyToShoot();
        isSplit = false;
       
    }
}
