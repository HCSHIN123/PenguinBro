using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet_RangeAttack : Bullet
{
    [SerializeField]
    protected float delayTime = 1.5f;
    [SerializeField]
    protected float bombRange = 30.0f;
    [SerializeField]
    protected ParticleSystem particle;

    abstract protected void Bomb();
    virtual protected IEnumerator COR_Bomb()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        particle.Play();
        yield return new WaitForSecondsRealtime(0.02f);
        Bomb();
        while (particle.isPlaying)
        {
            yield return null;
        }
        ReadyToShoot();
        transform.position = Vector3.zero;
    }
}
