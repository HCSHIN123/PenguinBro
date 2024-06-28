using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet_RangeAttack : Bullet
{
    [SerializeField]
    protected float bombTime = 0.15f;
    [SerializeField]
    protected float delayTime = 1.5f;
    [SerializeField]
    protected float bombRange = 30.0f;
    [SerializeField]
    protected ParticleSystem particle;
    protected bool isUsed = false;
    abstract protected void Bomb();
    virtual protected IEnumerator COR_Bomb()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        particle.Play();
        yield return new WaitForSecondsRealtime(0.15f);

        Bomb();
        while (particle.isPlaying)
        {
            yield return null;
        }
        StartCoroutine(COR_BulletOff());
    }

    public override void ReadyToShoot()
    {
        base.ReadyToShoot();
        isUsed = false;
    }
}
