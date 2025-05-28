using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private ObjectPool bulletPool;
    [SerializeField] private PooledObject bullet;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private void Awake() => Init();

    private void Init()
    {
        bulletPool = new ObjectPool(transform, bullet, 8);
    }

    public void Shoot()
    {
        Vector2 fireDir = Vector2.right;
        Bullet bullet = GetBullet();
        bullet.transform.position = transform.position;
        if (!playerSpriteRenderer.flipX)
        {
            bullet.bulletSR.flipX = false;
            bullet.Launch(fireDir);
        }
        else
        {
            bullet.bulletSR.flipX = true;
            fireDir = Vector2.left;
            bullet.Launch(fireDir);
        }
    }

    public Bullet GetBullet()
    {
        PooledObject po = bulletPool.PopPool();
        return po as Bullet;
    }
}
