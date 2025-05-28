using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] BulletController bulletController;

    private void Awake()
    {
        bulletController = GetComponent<BulletController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bulletController.Shoot();
        }
    }
}
