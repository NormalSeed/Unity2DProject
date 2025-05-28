using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private ObjectPool bulletPool;
    [SerializeField] private PooledObject bullet;

    private void Awake() => Init();

    private void Init()
    {
        bulletPool = new ObjectPool(transform, bullet, 8);
    }

    public void Shoot()
    {
        Bullet bullet = GetBullet();
        bullet.transform.position = transform.position;
    }

    public Bullet GetBullet()
    {
        PooledObject po = bulletPool.PopPool();
        return po as Bullet;
    }
}
