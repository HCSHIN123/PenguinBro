using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bullet_Bomb : Bullet_RangeAttack
{
    [SerializeField]
    private float nuckBackForce = 10f;
    
    override public void Init()
    {
        base.Init();
        isUsed = false;
    }

    protected override void Bomb()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, bombRange, targetMask);
        if (target.Length >= 1)
            target[0].GetComponent<PlayerDamaged>().NuckBack(nuckBackForce, transform.position);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.collider.CompareTag("Breakable") && !isUsed)
        {
            isUsed = true;
            StartCoroutine(COR_Bomb());
        }
    }


}
