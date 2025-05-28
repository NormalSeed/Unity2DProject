using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    private float bulletSpd = 20f;
    private Rigidbody2D rb;
    private float holdingTime = 0.5f;
    private float restTime;

    private void Awake() => Init();

    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = transform.right * bulletSpd;
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
