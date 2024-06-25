using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NuckBack(float _force, Vector3 _bombPos)
    {
        Vector2 dir = new Vector2(transform.position.x, transform.position.z) - new Vector2(_bombPos.x, _bombPos.z);
        dir.Normalize();
        Vector3 nuckbackDir = new Vector3(dir.x, 0f, dir.y);
        rb.AddForce(nuckbackDir * _force,ForceMode.Impulse);
    }
}
