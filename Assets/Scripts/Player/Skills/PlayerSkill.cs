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
        if (cooldownTimer <= 0)
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

public enum eSkill
{
    Q, W, E, R
}

public class Q_Skill : PlayerSkill
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
        yield return new WaitForSeconds(0.1f);
        bulletController.Shoot();
        yield return new WaitForSeconds(0.5f);
    }
}

public class W_Skill : PlayerSkill
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
        yield return new WaitForSeconds(0.5f);
    }
}

public class E_Skill : PlayerSkill
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

public class R_Skill : PlayerSkill
{
    // TODO: 플레이어를 중심으로 범위를 설정해 범위 안에 있는 적들을 플레이어에게서 천천히 멀어지게 하고 데미지를 주는 스킬
}