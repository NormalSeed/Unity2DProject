using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : PooledObject
{
    private float bulletSpd = 20f;
    private Rigidbody2D rb;
    private float holdingTime = 0.5f;
    private float restTime;
    public SpriteRenderer bulletSR;
    private int damage = 1;

    private void Awake() => Init();

    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletSR = GetComponent<SpriteRenderer>();
    }

    public void Launch(Vector2 fireDir)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(fireDir * bulletSpd, ForceMode2D.Impulse); 
        restTime = holdingTime;
    }

    private void Update()
    {
        restTime -= Time.deltaTime;

        if (restTime <= 0)
        {
            ReturnPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                ReturnPool();
            }
        }
        else
        {
            ReturnPool();
        }
    }
}
