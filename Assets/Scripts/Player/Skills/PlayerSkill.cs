using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    protected float coolTime;
    protected float cooldownTimer;
    
    protected virtual void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer >= 0)
        {
            UseSkill();
            cooldownTimer = coolTime;
            return true;
        }
        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {

    }
}

public class QSkill : PlayerSkill
{
    private BulletController bulletController;

    private void Awake()
    {
        bulletController = GetComponent<BulletController>();
        coolTime = 4.2f;
    }

    public override void UseSkill()
    {
        StartCoroutine(CoShootTwice());
    }

    IEnumerator CoShootTwice()
    {
        bulletController.Shoot();
        yield return new WaitForSeconds(0.2f);
        bulletController.Shoot();
        yield break;
    }
}

public class WSkill : PlayerSkill
{
    private BulletController bulletController;

    private void Awake()
    {
        bulletController = GetComponent<BulletController>();
        coolTime = 4.2f;
    }

    public override void UseSkill()
    {
        StartCoroutine(CoShootAll());
    }

    IEnumerator CoShootAll()
    {
        for (int i = 0; i < 8; i++)
        {
            bulletController.Shoot();
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}

public class ESkill : PlayerSkill
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coolTime = 7.7f;
    }

    public override void UseSkill()
    {
        rb.AddForce(transform.right * 10, ForceMode2D.Impulse);
    }
}
