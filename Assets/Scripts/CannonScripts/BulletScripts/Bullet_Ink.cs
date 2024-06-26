using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ink : Bullet_RangeAttack
{
    protected override void Bomb()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, bombRange, targetMask);
        if (target.Length >= 1)
            target[0].GetComponent<PlayerDamaged>().InkToScreen(); // RPC로 상대한테만
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.collider.CompareTag("Breakable") && !isUsed)
        {
            isUsed = true;
            Bomb();
        }
    }



}
