using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = 20 * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    
}
