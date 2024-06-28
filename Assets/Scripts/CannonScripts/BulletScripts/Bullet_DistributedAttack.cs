using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet_DistributedAttack : Bullet
{
    protected Transform[] bulletTransforms;
    protected bool isSplit = false;
}
