using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    private float bulletSpd = 20f;
    private Rigidbody2D rb;
    private float holdingTime = 0.5f;
    private float restTime;
    public SpriteRenderer bulletSR;

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
}
