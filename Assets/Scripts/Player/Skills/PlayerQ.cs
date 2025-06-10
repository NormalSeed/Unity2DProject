using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQ : Skill
{
    [SerializeField] private float fFireDelay;
    private WaitForSeconds fireDelay;
    public Coroutine coDoubleShot;

    protected override void Init()
    {
        base.Init();
        fireDelay = new WaitForSeconds(fFireDelay);
    }

    public override void UseSkill()
    {
        coDoubleShot = StartCoroutine(CoDoubleShot());
    }

    IEnumerator CoDoubleShot()
    {
        bulletController.Shoot();
        yield return fireDelay;
        bulletController.Shoot();
    }
}
