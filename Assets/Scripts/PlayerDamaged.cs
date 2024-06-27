using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField]
    private Image ink = null;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void NuckBack(float _force, Vector3 _bombPos) //RPC
    {
        Vector2 dir = new Vector2(transform.position.x, transform.position.z) - new Vector2(_bombPos.x, _bombPos.z);
        dir.Normalize();
        Vector3 nuckbackDir = new Vector3(dir.x, 0f, dir.y);
        rb.AddForce(nuckbackDir * _force, ForceMode.Impulse);
    }

    public void InkToScreen()
    {
        ink.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
