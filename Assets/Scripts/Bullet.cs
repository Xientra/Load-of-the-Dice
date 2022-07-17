using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    int damage = 0;
    private int amount = 0;

    private Gun.Chamber effects;
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

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Hit(damage);
            if (effects.GetPierce() && amount > 0)
            {
                int mult = effects.GetMultiplier();
                damage = Mathf.Abs(mult) == 1 ? damage + effects.GetMultiplier() : (Mathf.Sign(mult) == 1 ? damage * 2 : damage / 2);
                amount--;
            }
            else if (effects.GetRebound() && amount > 0)
            {
                GameObject closest = FindClosestEnemy();
                if (closest != null)
                {
                    transform.up = (closest.transform.position - transform.position);
                    rb.velocity = 20 * transform.up;
                }
                int mult = effects.GetMultiplier();
                damage = Mathf.Abs(mult) == 1 ? damage + effects.GetMultiplier() : (Mathf.Sign(mult) == 1 ? damage * 2 : damage / 2);
                amount--;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (collision.CompareTag("Wall"))
        {
            if (effects.GetRicochet() && amount > 0)
            {
                transform.up = Vector2.Reflect(transform.up, new Vector2(0, 1));
                rb.velocity = transform.up * 20;
                int mult = effects.GetMultiplier();
                damage = Mathf.Abs(mult) == 1 ? damage + effects.GetMultiplier() : (Mathf.Sign(mult) == 1 ? damage * 2 : damage / 2);
                amount--;
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetEffects(Gun.Chamber chamber)
    {
        effects = chamber;
        amount = effects.GetAmount();
    }

    public GameObject FindClosestEnemy()
    {
        //int maxRange = 10;
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        GameObject secondClosest = null;
        bool first = true;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                if (!first)
                {
                    secondClosest = closest;
                }
                closest = go;
                distance = curDistance;
                first = false;
            }
        }
        //return distance <= maxRange ? closest : null;
        return secondClosest;
    }


}
