using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bullet_Bomb : Bullet
{
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    float nuckBackForce = 10f;
    [SerializeField]
    float delayTime = 1.5f;
    [SerializeField]
    float bombRange = 2.0f;
    [SerializeField]
    private ParticleSystem particle;

    private bool isBomb = false;
    override public void Init()
    {
        base.Init();
        isBomb = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    private IEnumerator COR_Bomb()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        particle.Play();
        Bomb();
        while(particle.isPlaying)
        {
            yield return null;
        }

        Init();
        transform.position = Vector3.zero;
    }

    private void Bomb()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;

        Collider[] target = Physics.OverlapSphere(transform.position, bombRange, targetMask);
        if (target.Length >= 1)
            target[0].GetComponent<Player>().NuckBack(nuckBackForce, transform.position);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.collider.CompareTag("Breakable") && !isBomb)
        {
            isBomb = true;
            StartCoroutine(COR_Bomb());
        }
    }


}
