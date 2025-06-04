using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerW : Skill
{
    private BulletController bulletController;
    [SerializeField] private float fFireDelay;
    private WaitForSeconds fireDelay;
    private Coroutine coRapidFire;

    protected override void Init()
    {
        base.Init();
        fireDelay = new WaitForSeconds(fFireDelay);
        bulletController = player.GetComponentInChildren<BulletController>();
    }

    public override void UseSkill()
    {
        StartCoroutine(CoRapidFire());
    }

    IEnumerator CoRapidFire()
    {
        for (int i = 0; i < 8; i++)
        {
            bulletController.Shoot();
            yield return fireDelay;
        }
    }
}
