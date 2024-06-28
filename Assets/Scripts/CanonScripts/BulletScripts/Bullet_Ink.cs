using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ink : Bullet_RangeAttack
{
    protected override void Bomb()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, bombRange, targetMask);
        if (target.Length >= 1)
            target[0].GetComponent<PlayerDamaged>().InkToScreen(); // RPC�� ������׸�
    }

    protected override void OnCollisionBulletEvent(Collision collision)
    {
       
        if (collision.collider.CompareTag(breakableTag) && !isUsed)
        {
            isHited = true;
            isUsed = true;
            StartCoroutine(COR_Bomb());
            // Bomb();
        }
    }

}
